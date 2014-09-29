using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NetworkLib;

namespace VSY_Client
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IClient
    {
        private ClientLink _link;

        public MainWindow()
        {
            InitializeComponent();
            Connect(System.Environment.MachineName);
        }

        private void Connect(String server)
        {
            try
            {
                _link = new ClientLink(this);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _link.WriteMessage(messageBox.Text, IPAddress.Loopback);
            messageBox.Text = "";
        }

        public void ActionAfterRead(Packet receipt)
        {
            Dispatcher.BeginInvoke(new Action(() => receivedMessageBox.Text += receipt.Content + "\n"));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _link.Close();
            _link = null;
        }
    }
}
