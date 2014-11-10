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

namespace ProxyGzip
{
	public class Router
	{
		private Socket outAddr;
		private string targetAddr;
		private int outPort;
		private ILog log;
		private GZipFilter gzipFilter;
		public Router(Socket inSocket, string outAddr, int outPort)
		{
			this.outAddr = inSocket;
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
				Socket targetSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				log.Debug("Connecting to Target...");
				targetSocket.Connect(targetAddr, outPort);

				var proxy = new Proxy<HeaderedWrapper, HeaderedWrapper>(outAddr, targetSocket);

				if (ProxyConfig.Source.Gzip)
					proxy.SourceWrapper.AddFilter(gzipFilter);
				if (ProxyConfig.Target.Gzip)
					proxy.TargetWrapper.AddFilter(gzipFilter);

				proxy.SourceWrapper.OnReceived += SourceWrapper_OnReceived;
				proxy.SourceWrapper.OnFilterError += SourceWrapper_OnFilterError;

				proxy.TargetWrapper.OnReceived += TargetWrapper_OnReceived;
				proxy.TargetWrapper.OnFilterError += TargetWrapper_OnFilterError;

				proxy.Start();
				proxy.WaitEnd();
			}
			catch (Exception ex)
			{
				log.Error("Error on router", ex);
			}
		}

		void TargetWrapper_OnFilterError(FilterException filterError)
		{
			log.Error("Filter error on Target", filterError);
		}

		void SourceWrapper_OnFilterError(FilterException filterError)
		{
			log.Error("Filter error on Source", filterError);
		}

		void TargetWrapper_OnReceived(byte[] bytes)
		{
			if (ProxyConfig.Sniffer.ShowOnConsole)
				log.InfoFormat("Target -> Source: {0}", Encoding.Default.GetString(bytes));
			if (ProxyConfig.Sniffer.Save)
				Sniffer.Save("Request", bytes);
		}

		void SourceWrapper_OnReceived(byte[] bytes)
		{
			if (ProxyConfig.Sniffer.ShowOnConsole)
				log.InfoFormat("Source -> Target: {0}", Encoding.Default.GetString(bytes));
			if (ProxyConfig.Sniffer.Save)
				Sniffer.Save("Response", bytes);
		}
	}
}
