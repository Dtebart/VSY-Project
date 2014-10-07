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
    class ServeThread
    {
        Thread thread;
        ServerLink _serverLink;

        public ServeThread(ServerLink link){
            _serverLink = link;
        }

        public void serve()
        {
            while (_serverLink.Open())
            {
                Packet Complete_message = ReadMessage();
                HandleMessage(Complete_message);
            }
        }

        public void start()
        {
            thread.Start();
        }

        private Packet ReadMessage()
        {
            Packet receipt = _serverLink.ReadChannel();
            return receipt;
        }

        private void HandleMessage(Packet reciept)
        {
            _serverLink.WriteMessage(System.Text.Encoding.UTF8.GetString(reciept.Bytes), IPAddress.Loopback);
        }
    }
}
