using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyGzip.Config
{
	public class ConnectionInfo
	{
		public string Address { get; set; }
		public int Port { get; set; }
		public bool Gzip { get; set; }
	}
}
