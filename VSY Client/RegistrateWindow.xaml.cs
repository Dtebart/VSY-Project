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
        public delegate void ResponseAction(String response);
        private ClientLink _link;
        public RegistrateWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow chatWindow = new MainWindow(userNameTextBox.Text);
                _link = new ClientLink(chatWindow);
                Packet registrateRequest = new Packet(userNameTextBox.Text, userNameTextBox.Text, "Registration", MessageTypes.Registrate);
                registrateRequest.AddParam(passwordTextBox.Password);
                _link.WriteMessage(registrateRequest);

                chatWindow._link = _link;

                Window mainWindow = App.Current.MainWindow;

                mainWindow.Close();
                Close();
                chatWindow.Show();
            }
            catch (ArgumentNullException excep)
            {
                Console.WriteLine("ArgumentNullException: {0}", excep);
            }
            catch (SocketException excep)
            {
                Console.WriteLine("SocketException: {0}", excep);
            }
        }
    }
}
