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
                try
                {
                    Packet receipt = _serverLink.ReadChannel();
                    HandleMessage(receipt);
                }
                catch (IndexOutOfRangeException e)
                {
                    LogoutUser(_serverLink._clientName);
                }
            }
        }

        public void start()
        {
            _thread.Start();
        }

        private void HandleMessage(Packet receipt)
        {
            try
            {
                RequestHandler requestHandler = RequestHandler.GetHandler(receipt.Type);
                Packet processedPacket = requestHandler.HandleRequest(receipt, _serverLink);
                TcpClient receiver = _serverLink.ClientList[processedPacket.DestUser];
                _serverLink.WriteMessage(processedPacket, receiver.GetStream());
            }
            catch (InvalidUserException e)
            {
                // Send error Message back to Client
                Packet errorResponse = new Packet(receipt.SrcUser, "ERROR", MessageTypes.Login);
                _serverLink.WriteMessage(errorResponse, e._client.GetStream());
                _serverLink.Client.Close();
            }
        }

        private void LogoutUser(string userName)
        {
            TcpClient closingClient = _serverLink.ClientList[userName];

            closingClient.Close();
            ServerLink.RemoveClient(userName);
            Console.WriteLine("Closed Connection to Client: {0}\n", userName);

            UserDBApp dbApp = new UserDBApp("Data Source=DANIEL-PC\\SQLEXPRESS;", "Initial Catalog=UserDB;");
            dbApp.ChangeOnlinestatus(userName, false);
        }
    }
}
