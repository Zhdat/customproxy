using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxyGzip.Config.SettingTypes;

namespace ProxyGzip.Config
{
	public class ConnectionInfo
	{
      	public string Address { get; set; }
		public int Port { get; set; }
        public bool Gzip { get; set; }
	}
}
