using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProxyGzip.Config;

namespace ProxyGzip
{
    internal static class Sniffer
    {
        static int counter = 0;
        public static void Save(string title, byte[] data)
        {
            int num = counter++;
            string full_path = Path.Combine(ProxyConfig.Sniffer.Directory, string.Format("{0}-{1}-{2}.{3}", ProxyConfig.Source.Port, counter, title, ProxyConfig.Sniffer.Extension));
            if (!Directory.Exists(ProxyConfig.Sniffer.Directory))
                Directory.CreateDirectory(ProxyConfig.Sniffer.Directory);
            File.WriteAllText(full_path, Encoding.Default.GetString(data));
        }
    }
}
