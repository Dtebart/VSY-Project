﻿using System;
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
        private String _receiver;

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
            _link.WriteMessage(messageBox.Text, _receiver);
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

        private void addFriendButton_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem newFriend = new ListBoxItem();
            newFriend.Content = addFriendTextBox.Text;
            newFriend.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(friendListItem_MouseDown), true);
            addFriendTextBox.Text = String.Empty;
            friendsListBox.Items.Add(newFriend);
        }

        private void friendListItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem selectedFriend = (ListBoxItem)e.Source;

            _receiver = selectedFriend.Content.ToString();
            int buttonTextPlaceStart = sendButton.Content.ToString().IndexOf("-");
            int buttonTextPlaceEnd = sendButton.Content.ToString().LastIndexOf("-");
            String oldReceiver = sendButton.Content.ToString().Substring(buttonTextPlaceStart + 1, buttonTextPlaceEnd - buttonTextPlaceStart - 1);
            sendButton.Content = sendButton.Content.ToString().Replace(oldReceiver, _receiver);
        }

        private void friendsListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
