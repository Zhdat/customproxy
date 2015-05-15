using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyGzip.Config
{
	public class SnifferInfo
	{
		public bool Save { get; set; }
		public bool ShowOnConsole { get; set; }
		public string Directory { get; set; }
		public string Extension { get; set; }
        public string InputDescription { get; set; }
        public string OutputDescription { get; set; }
	}
}
