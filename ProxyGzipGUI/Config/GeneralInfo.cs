using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyGzipGUI.Config
{
    internal class GeneralInfo
    {
        public GeneralInfo()
        {
            UseHeader = true;
        }
        public bool UseHeader { get; set; }
    }
}
