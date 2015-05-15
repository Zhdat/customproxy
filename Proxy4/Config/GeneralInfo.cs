using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyGzip.Config
{
    public class GeneralInfo
    {
        public GeneralInfo()
        {
            UseHeader = true;
        }
        public bool UseHeader { get; set; }
    }
}
