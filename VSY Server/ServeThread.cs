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
        private Thread _thread;
        private ServerLink _serverLink;
        private Dictionary<IPAddress, TcpClient> _clientList;
        private bool _knownUser;

        public ServeThread(ServerLink link, Dictionary<IPAddress, TcpClient> clientList){
            _serverLink = link;
            _clientList = clientList;
            _knownUser = false;

            ThreadStart serveDel = new ThreadStart(serve);
            _thread = new Thread(serveDel);
        }

        public void serve()
        {
            while (_serverLink.Open())
            {
                Packet receipt = ReadMessage();

                if (_knownUser == false)
                {
                    Monitor.Enter(_clientList);
                        _clientList.Add(receipt.SrcIp, _serverLink.Client);
                    Monitor.Exit(_clientList);
                    _knownUser = true;
                }

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
            TcpClient receiver = _clientList[receipt.DestIp];
            _serverLink.WriteMessage(receipt, receiver.GetStream());
        }
    }
}
