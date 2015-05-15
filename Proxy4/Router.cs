using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using EasySocket;
using log4net;
using EasySocket.Filters;
using EasySocket.Exceptions;
using ProxyGzip.Config;
using EasySocket.Events;
using ProxyGzip.Config.SettingTypes;

namespace ProxyGzip
{
    internal class Router
    {
        private Socket inSocket;
        private string targetAddr;
        private int outPort;
        private ILog log;
        private GZipFilter gzipFilter;
        public Router(Socket inSocket, string outAddr, int outPort)
        {
            this.inSocket = inSocket;
            this.targetAddr = outAddr;
            this.outPort = outPort;

            log = LogManager.GetLogger(this.GetType());

            this.gzipFilter = new GZipFilter();
        }
        public void Run()
        {
            try
            {
                log.Debug("Starting router...");
                var outSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                log.Info("Connecting to Target...");
                outSocket.Connect(targetAddr, outPort);
                log.Info("Connected!");

                IProxy proxy;
                if (ProxyConfig.General.UseHeader)
                {
                    var inWrapper = new HeaderedWrapper(inSocket);
                    if (ProxyConfig.Source.Gzip) inWrapper.AddFilter(gzipFilter);

                    var outWrapper = new HeaderedWrapper(outSocket);
                    if (ProxyConfig.Target.Gzip) outWrapper.AddFilter(gzipFilter);

                    var headeredProxy = new Proxy<HeaderedWrapper, HeaderedWrapper>(inWrapper, outWrapper);
                    proxy = headeredProxy;

                    headeredProxy.SourceWrapper.OnFilterError += SourceWrapper_OnFilterError;
                    headeredProxy.TargetWrapper.OnFilterError += TargetWrapper_OnFilterError;
                }
                else
                {
                    proxy = new Proxy<SimpleWrapper, SimpleWrapper>(inSocket, outSocket, s => new SimpleWrapper(s), s => new SimpleWrapper(s));
                    log.Info("The header cannot operate with filters in No-Header mode");
                }
                proxy.SourceWrapper.OnDataReceived += SourceWrapper_OnDataReceived;
                proxy.TargetWrapper.OnDataReceived += TargetWrapper_OnDataReceived;

                proxy.Start();
                proxy.WaitEnd();
            }
            catch (Exception ex)
            {
                //log.ErrorFormat("Router error: {0}", ex.Message);
                inSocket.Close();
                //log.Info("Client socket closed!");
            }
        }

        void TargetWrapper_OnDataReceived(object sender, WrapperDataReceivedEventArgs e)
        {
            if (ProxyConfig.Sniffer.ShowOnConsole)
            {
                log.InfoFormat("T->S: {0}", e.DataString);
            }
            else
            {
                log.InfoFormat("T->S: {0} bytes", e.DataBytes.Length);
            }
            if (ProxyConfig.Sniffer.Save)
            {
                Sniffer.Save(ProxyConfig.Sniffer.OutputDescription, e.DataBytes);
            }
        }

        void SourceWrapper_OnDataReceived(object sender, WrapperDataReceivedEventArgs e)
        {
            if (ProxyConfig.Sniffer.ShowOnConsole)
            {
                log.InfoFormat("S->T: {0}", e.DataString);
            }
            else
            {
                log.InfoFormat("S->T: {0} bytes", e.DataBytes.Length);
            }
            if (ProxyConfig.Sniffer.Save)
                Sniffer.Save(ProxyConfig.Sniffer.InputDescription, e.DataBytes);
        }

        void TargetWrapper_OnFilterError(FilterException filterError)
        {
            log.ErrorFormat("Filter error on Target: {0}", filterError.Message);
        }

        void SourceWrapper_OnFilterError(FilterException filterError)
        {
            log.ErrorFormat("Filter error on Source: {0}", filterError.Message);
        }
    }
}
