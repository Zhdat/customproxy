using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ProxyGzip;

namespace ProxyGzipStarter
{
    public partial class ProxyGzipService : ServiceBase
    {
        private Proxy proxy;
        public ProxyGzipService()
        {
            InitializeComponent();
            this.proxy = new Proxy();    
        }

        protected override void OnStart(string[] args)
        {
            proxy.StartAsync();
        }

        protected override void OnStop()
        {
            proxy.Stop();
        }
    }
}
