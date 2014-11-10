using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpConfig;

namespace ProxyGzip.Config
{
	internal static class ProxyConfig
	{
		static ProxyConfig()
		{
			Reload();
		}
		public static void Reload()
		{
			try
			{
				var config = Configuration.LoadFromFile("./ProxyGzip.ini");
				Source = config["Source"].CreateObject<ConnectionInfo>();
				Target = config["Target"].CreateObject<ConnectionInfo>();
				Sniffer = config["Sniffer"].CreateObject<Snifferinfo>();
			}
			finally { }
		}
		public static ConnectionInfo Source { get; private set; }
		public static ConnectionInfo Target { get; private set; }
		public static Snifferinfo Sniffer { get; private set; }
	}
}
