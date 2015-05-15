using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpConfig;
using System.Reflection;
using System.IO;

namespace ProxyGzip.Config
{
    public static class ProxyConfig
    {
        readonly static string FILE;
        static ProxyConfig()
        {
            FILE = Path.Combine(new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName, "ProxyGzip.ini");
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
            //catch { }
            finally { }
        }
        public static ConnectionInfo Source { get; private set; }
        public static ConnectionInfo Target { get; private set; }
        public static SnifferInfo Sniffer { get; private set; }
        public static GeneralInfo General { get; private set; }
    }
}
