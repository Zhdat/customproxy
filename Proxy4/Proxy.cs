using System.Net;
using System.Net.Sockets;
using System.Threading;
using EasySocket.Filters;
using log4net;
using ProxyGzip.Config;
using System;
using EasySocket.Events;
using System.Text;

namespace ProxyGzip
{
    public class Proxy
    {
        private ILog log;
        private int sourcePort;
        private string targetAddr;
        private int targetPort;
        private bool running;

        public Proxy()
            : this(ProxyConfig.Source.Port, ProxyConfig.Target.Address, ProxyConfig.Target.Port)
        {

        }
        public Proxy(int sourcePort, string targetAddr, int targetPort)
        {
            this.log = LogManager.GetLogger(typeof(Proxy));
            Thread.CurrentThread.Name = "Proxy";

            this.sourcePort = sourcePort;
            this.targetAddr = targetAddr;
            this.targetPort = targetPort;

            this.Encoding = Encoding.Default;

            System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
        }

        /// <summary>
        /// Run the proxy synchronously
        /// </summary>
        public void Run()
        {
            this.running = true;

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  
            listener.Bind(new IPEndPoint(IPAddress.Any, sourcePort));
            listener.Listen(10);


            log.InfoFormat("Proxy started. Gzip: source={0} target={1}", ProxyConfig.Source.Gzip, ProxyConfig.Target.Gzip);
            log.Info("Awaiting connection");
            while (this.running)
            {
                Socket inSocket = listener.Accept();
                Router rtargeter = new Router(inSocket, targetAddr, targetPort);
                Thread thread = new Thread(rtargeter.Run);
                thread.Name = "Router";
                thread.Start();
            }
        }

        /// <summary>
        /// Start the proxy asynchronously
        /// </summary>
        public void StartAsync()
        {
            Thread thread = new Thread(Run);
            thread.Name = "Proxy";
            thread.Start();
        }

        public void Stop()
        {
            this.running = false;
        }

        public Encoding Encoding { get; set; }
        event EventHandler<WrapperDataReceivedEventArgs> OnDataReceived;
        private void DataReceived(byte[] data)
        {
            if (OnDataReceived != null)
            {
                var eventArgs = new WrapperDataReceivedEventArgs(data, this.Encoding);
                OnDataReceived(this, eventArgs);
            }
        }
    }
}
