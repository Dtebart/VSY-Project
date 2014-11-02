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
    public partial class RegistrateWindow : Window, IClient
    {
        private ClientLink _link;
        public delegate void ResponseAction(String response);
        public RegistrateWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (passwordTextBox.Password != "" && passwordTextBox.Password == passwordConfirmTextBox.Password)
                {
                    _link = new ClientLink(this);
                    Packet registrateRequest = new Packet(userNameTextBox.Text, userNameTextBox.Text, "Registration", MessageTypes.Registrate);
                    registrateRequest.AddParam(passwordTextBox.Password);
                    _link.WriteMessage(registrateRequest);
                }
                else
                {
                    MessageBox.Show("Passwörter stimmen nicht überein!");
                    passwordTextBox.Password = "";
                    passwordConfirmTextBox.Password = "";
                }
            }
            catch (ArgumentNullException excep)
            {
                Console.WriteLine("ArgumentNullException: {0}", excep);
            }
            catch (SocketException excep)
            {
                Console.WriteLine("SocketException: {0}", excep);
                MessageBox.Show("Keine Antwort vom Server. Bitte versuchen sie es Später erneut.");
            }
        }

        public void ActionAfterRead(Packet receipt)
        {
            ResponseAction reg = Registrate;
            Dispatcher.Invoke(reg, receipt.Content);
        }

        private void Registrate(String response)
        {
            if (!response.Equals("ERROR\n"))
            {
                MainWindow chatWindow = new MainWindow(userNameTextBox.Text);
                _link._iClient = chatWindow;
                chatWindow._link = _link;
                Window mainWindow = App.Current.MainWindow;
                mainWindow.Close();
                Close();
                chatWindow.Show();
            }
            else
            {
                _link.Close();
                MessageBox.Show("Der Name ist bereits vergeben.");
            }
        }

        public void Disconnected()
        {

        }
    }
}
