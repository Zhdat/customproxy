using System.Net;
using System.Net.Sockets;
using System.Threading;
using EasySocket.Filters;
using log4net;
using ProxyGzip.Config;

namespace ProxyGzip
{
    public class Proxy
    {
        private ILog log;
        private int sourcePort;
        private string targetAddr;
        private int targetPort;
        private GZipFilter gzipFilter;
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
            this.gzipFilter = new GZipFilter();

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

            log.Debug("Awaiting connection");
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
    }
}
