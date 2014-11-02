using System;
using Microsoft.Win32;
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
using System.Media;

using NetworkLib;

namespace VSY_Client
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IClient
    {
        public ClientLink _link;
        private String _receiver;
        private String _userName;
        private String _password;
        private ChatHistory _chatHistory;
        private SoundPlayer _recievedMessageInformation;
        public delegate void ResponseAction(String response);
        public MainWindow(String userName, String Password)
        {
            _receiver = "";            
            InitializeComponent();
            _userName = userName;
            _chatHistory = new ChatHistory();
            _recievedMessageInformation = new SoundPlayer(Properties.Resources.gotMessage);
            _password = Password;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (messageBox.Text != "" && _receiver != "")
            {
                if (!_chatHistory.UserExist(_receiver))
                    _chatHistory.AddUser(_receiver);
                Packet messageRequest = new Packet(_userName, _receiver, messageBox.Text, MessageTypes.TextMessage);
                _link.WriteMessage(messageRequest);
                _chatHistory.AddTexttoUser("Ich: " + messageRequest.Content, messageRequest.DestUser);
                receivedMessageBox.Text = _chatHistory.GetText(messageRequest.DestUser);
                messageBox.Text = "";
            }
        }
        public void ActionAfterRead(Packet receipt)
        {
            if (receipt.Type == MessageTypes.TextMessage)
            {
                if (!_chatHistory.UserExist(receipt.SrcUser))
                    _chatHistory.AddUser(receipt.SrcUser);
                _chatHistory.AddTexttoUser(receipt.SrcUser + ": " + receipt.Content, receipt.SrcUser);
                ResponseAction updateMessageBox = UpdateRecievedMessages;
                Dispatcher.Invoke(updateMessageBox, receipt.SrcUser);
                

            }
            else if (receipt.Type == MessageTypes.GetFriendlist)
            {
                List<String> friendlist = receipt.AdditionalArgs;
                for (int i = 0; i < friendlist.Count; i += 2)
                {
                    ResponseAction addFriend = AddFriendEntry;
                    Dispatcher.Invoke(addFriend, friendlist[i] + "," + friendlist[i+1]);
                }
            }
            else if (receipt.Type == MessageTypes.AddFriend)
            {
                if (!receipt.Content.Equals("ERROR\n"))
                {
                    ResponseAction addFriend = AddFriendEntry;
                    Dispatcher.Invoke(addFriend, receipt.Content.Replace("\n", "") + "," + receipt.AdditionalArgs[0]);
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("Name konnte nicht gefunden werden.")));
                }
            }
            else if (receipt.Type == MessageTypes.FriendOnline)
            {
                ResponseAction changeOnlineStatus = ChangeOnlineStatus;
                Dispatcher.Invoke(changeOnlineStatus, receipt.SrcUser + "," + "True");
            }
            else if (receipt.Type == MessageTypes.FriendOffline)
            {
                ResponseAction changeOnlineStatus = ChangeOnlineStatus;
                Dispatcher.Invoke(changeOnlineStatus, receipt.SrcUser + "," + "False");
            }
        }

        public void UpdateRecievedMessages(string user)
        {
            receivedMessageBox.Text = _chatHistory.GetText(user);
            _recievedMessageInformation.Play();
        }

        private void ChangeOnlineStatus(string userInfo)
        {
            string[] userInf = userInfo.Split(',');
            foreach (ListBoxItem friend in friendsListBox.Items)
            {
                if (friend.Content.Equals(userInf[0]))
                {
                    if (Convert.ToBoolean(userInf[1]))
                    {
                        friend.Background = Brushes.Green;
                    }
                    else
                    {
                        friend.Background = Brushes.Red;
                    }
                }
            }
        }

        private void FetchFriendlist()
        {
            Packet friendlistRequest = new Packet(_userName, _userName, "GetFriends", MessageTypes.GetFriendlist);
            _link.WriteMessage(friendlistRequest);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _link.Close();
            _link = null;
        }
        private void addFriendButton_Click(object sender, RoutedEventArgs e)
        {
            if (addFriendTextBox.Text != String.Empty)
            {
                Packet addFriendRequest = new Packet(_userName, _userName, addFriendTextBox.Text, MessageTypes.AddFriend);
                _link.WriteMessage(addFriendRequest);
                addFriendTextBox.Text = String.Empty;
            }
        }
        private void AddFriendEntry(string userInfo)
        {
            String[] userInf = userInfo.Split(',');
            String name = userInf[0];
            Boolean isOnline = Convert.ToBoolean(userInf[1]);

            ListBoxItem newFriend = new ListBoxItem();

            if (isOnline)
            {
                newFriend.Background = Brushes.Green;
            }
            else
            {
                newFriend.Background = Brushes.Red;
            }
            newFriend.Content = name;
            newFriend.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(friendListItem_MouseDown), true);
            friendsListBox.Items.Add(newFriend);
            
            if (!_chatHistory.UserExist(name))
                _chatHistory.AddUser(name);
        }
        private void friendListItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem selectedFriend = (ListBoxItem)e.Source;
            _receiver = selectedFriend.Content.ToString();
            int buttonTextPlaceStart = sendButton.Content.ToString().IndexOf("-");
            int buttonTextPlaceEnd = sendButton.Content.ToString().LastIndexOf("-");
            String oldReceiver = sendButton.Content.ToString().Substring(buttonTextPlaceStart + 1, buttonTextPlaceEnd - buttonTextPlaceStart - 1);
            sendButton.Content = sendButton.Content.ToString().Replace(oldReceiver, _receiver);
            if (!_chatHistory.UserExist(_receiver))
                _chatHistory.AddUser(_receiver);
            receivedMessageBox.Text = _chatHistory.GetText(_receiver);
        }
        private void friendsListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
        }


        private void Reconnect(string password)
        {
            
            messageBox.Text = "Reconnecting...";
            _link = new ClientLink(this);
            Packet loginRequest = new Packet(_userName, _userName, "Login-Try", MessageTypes.Login);
            loginRequest.AddParam(password);
            _link.WriteMessage(loginRequest);
            

            messageBox.Text = "";
           // FetchFriendlist();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            FetchFriendlist();
        }
        public void Disconnected()
        {
            ResponseAction reconnect = Reconnect;
            Dispatcher.Invoke(reconnect, _password);
        }
    }
    
}
