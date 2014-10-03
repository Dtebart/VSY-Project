using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLib;
using System.Threading;

namespace VSY_Server
{
    public class ServeThread
    {
        Thread _thread;
        ServerLink _serverLink;
        TcpClient _client;

        public ServeThread(ServerLink link, TcpClient client)
        {
            _serverLink = link;
            this._client = client;
            _thread = new Thread(this.Serve);
        }

        public void Serve()
        {
            Packet message = ReadMessage();
            Console.WriteLine("Erhalten: {0}" , message.Content);
            //HandleMessage(message);
        }

        public void Start()
        {
            _thread.Start();
        }

        private Packet ReadMessage()
        {
            return _serverLink.ReadChannel();
        }

        private void HandleMessage(Packet receipt)
        {
            _serverLink.ConnectToClient(13001, IPAddress.Loopback);
            _serverLink.WriteMessage(receipt.Content, IPAddress.Loopback);
        }
    }
}
