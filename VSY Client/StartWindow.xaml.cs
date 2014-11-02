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
    /// Interaktionslogik für Start.xaml
    /// </summary>
    public partial class Start : Window, IClient
    {
        public delegate void ResponseAction(String response);
        private ClientLink _link;
        public Start()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String userName = userNameTextBox.Text;
            try
            {
                _link = new ClientLink(this);

                Packet loginRequest = new Packet(userName, userName, "Login-Try", MessageTypes.Login);
                loginRequest.AddParam(passwordTextBox.Password);
                _link.WriteMessage(loginRequest);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window registrateWindow = new RegistrateWindow();
            registrateWindow.Show();
        }

        public void ActionAfterRead(Packet receipt)
        {
            if (receipt.Type == MessageTypes.Login)
            {
                ResponseAction login = OnFeedbackLogin;
                Dispatcher.Invoke(login, receipt.Content);
            }
        }

        private void OnFeedbackLogin(String feedback)
        {
            if (feedback == "OK\n")
            {
                MainWindow chatWindow = new MainWindow(userNameTextBox.Text);
                chatWindow._link = _link;
                _link._iClient = chatWindow;

                Close();
                chatWindow.Show();
            }
            else
            {
                MessageBox.Show("Login-Daten inkorrekt!");
                _link.Close();
            }
        }

        public void Disconnected()
        {

        }
    }
}
