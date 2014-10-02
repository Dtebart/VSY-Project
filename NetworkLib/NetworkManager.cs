using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace NetworkLib
{
   public class NetworkManager
    {
        
        private ClientListener _clientlistener;
        private ClientLink _clientsender;
        public NetworkStream _stream;
        Thread _listenThread;
        private IClient _client;
        public NetworkManager(IClient client)
        {
            _clientlistener = new ClientListener(this);
            _clientsender = new ClientLink(this);
            _listenThread = new Thread(Listen);
            _listenThread.Start();
            _client = client;
        }



        public void Listen()
        {
            while (true)
            {
               Packet recievedPacket = _clientlistener.Listen();
               _client.ActionAfterRead(recievedPacket);
            }
        }

        public void SendMessage(string message)
        {
            _clientsender.WriteMessage(message, IPAddress.Loopback);
            
        }
    }
}
