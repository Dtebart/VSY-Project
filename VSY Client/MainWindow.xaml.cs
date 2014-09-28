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
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream messageChannel;
        private Thread readChannel;

        public MainWindow()
        {
            InitializeComponent();
            Connect(System.Environment.MachineName);
        }

        private void Connect(String server)
        {
            try
            {
                Int32 port = 13000;

                // Initialize connection to dedicated Server
                client = new TcpClient(server, port);
                messageChannel = client.GetStream();

                // Start thread for reading data
                ThreadStart readDel = new ThreadStart(ReadChannel);
                readChannel = new Thread(readDel);

                readChannel.Start();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WriteMessage("Hello World!", IPAddress.Loopback);
        }

        private void WriteMessage(string message, IPAddress destIp)
        {
            Packet packet = new Packet(destIp, message);

            // Send the message to the connected TcpServer. 
            messageChannel.Write(packet.Bytes, 0, packet.Bytes.Length);
        }

        private void ReadChannel()
        {
            // Buffer to store the response bytes.
            Byte[] data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            int i;
            while (true)
            {
                while ((i = messageChannel.Read(data, 0, data.Length)) != 0)
                {
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, i);

                    Dispatcher.BeginInvoke(new Action(() => messageBox.Text = responseData));
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            readChannel.Abort();
        }
    }
}
