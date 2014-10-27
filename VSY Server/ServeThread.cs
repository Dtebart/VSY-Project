using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLib;
using NetworkLib.RequestHandler;
using System.Threading;

namespace VSY_Server
{
    class ServeThread
    {
        private Thread _thread;
        private ServerLink _serverLink;

        public ServeThread(ServerLink link){
            _serverLink = link;


            ThreadStart serveDel = new ThreadStart(serve);
            _thread = new Thread(serveDel);
        }

        public void serve()
        {
            while (_serverLink.Open())
            {
                Packet receipt = ReadMessage();
                HandleMessage(receipt);
            }
        }

        public void start()
        {
            _thread.Start();
        }

        private Packet ReadMessage()
        {
            Packet receipt = _serverLink.ReadChannel();
            return receipt;
        }

        private void HandleMessage(Packet receipt)
        {
            RequestHandler requestHandler = RequestHandler.GetHandler(receipt.Type);
            Packet processedPacket = requestHandler.HandleRequest(receipt, _serverLink);
            TcpClient receiver = _serverLink.ClientList[processedPacket.DestUser];
            _serverLink.WriteMessage(processedPacket, receiver.GetStream());
        }
    }
}
