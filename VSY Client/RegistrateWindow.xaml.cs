using System;
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
using System.Windows.Shapes;
using System.Net.Sockets;
using NetworkLib;

namespace VSY_Client
{
    /// <summary>
    /// Interaktionslogik für RegistrateWindow.xaml
    /// </summary>
    public partial class RegistrateWindow : Window
    {
        public RegistrateWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow chatWindow = new MainWindow();
            ClientLink link;
            try
            {
                link = new ClientLink(chatWindow);
                chatWindow._link = link;
            }
            catch (ArgumentNullException excep)
            {
                Console.WriteLine("ArgumentNullException: {0}", excep);
            }
            catch (SocketException excep)
            {
                Console.WriteLine("SocketException: {0}", excep);
            }

            Window mainWindow = App.Current.MainWindow;

            Close();
            mainWindow.Close();
            chatWindow.Show();
        }
    }
}
