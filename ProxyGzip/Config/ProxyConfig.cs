using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpConfig;
using System.Reflection;
using System.IO;

namespace ProxyGzip.Config
{
    internal static class ProxyConfig
    {
        readonly static string FILE;
        static ProxyConfig()
        {
            FILE = Path.Combine(Assembly.GetEntryAssembly().Location, "..", "ProxyGzip.ini");
            Reload();
        }
        public static void Reload()
        {
            try
            {
                var config = Configuration.LoadFromFile(FILE);
                Source = config["Source"].CreateObject<ConnectionInfo>();
                Target = config["Target"].CreateObject<ConnectionInfo>();
                Sniffer = config["Sniffer"].CreateObject<SnifferInfo>();
                General = config["General"].CreateObject<GeneralInfo>();
            }
            finally { }
        }
        public static ConnectionInfo Source { get; private set; }
        public static ConnectionInfo Target { get; private set; }
        public static SnifferInfo Sniffer { get; private set; }
        public static GeneralInfo General { get; private set; }
    }
}
