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
            HandleMessage(message);
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
            _serverLink = GetLink(receipt);
            _serverLink.WriteMessage(receipt.Content, IPAddress.Loopback);
        }

        private ServerLink GetLink(Packet receipt)
        {
            // Test Settings!!
            return new ServerLink(13000, IPAddress.Loopback);
        }
    }
}
