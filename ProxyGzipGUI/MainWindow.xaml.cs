using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using ProxyGzip;
using ProxyGzip.Config;

namespace ProxyGzipGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtInputTitle.Text = ProxyConfig.Sniffer.InputDescription;
            txtOutputTitle.Text = ProxyConfig.Sniffer.OutputDescription;

            Proxy proxy = new Proxy();
            //proxy.
            proxy.StartAsync();
        }

  
    }
}
