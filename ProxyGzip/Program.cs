using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Config;
using System.IO;
using ProxyGzip.Config;

namespace ProxyGzip
{
	class Program
	{
		public static void Main(string[] args)
		{
			XmlConfigurator.ConfigureAndWatch(new FileInfo("./log.xml"));

			Proxy proxy = new Proxy(ProxyConfig.Source.Port, ProxyConfig.Target.Address, ProxyConfig.Target.Port);
			proxy.Run();
		}
	}
}
