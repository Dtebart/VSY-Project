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
        ServerLink serverLink;
        TcpClient client;

        public ServeThread(ServerLink link, TcpClient client)
        {
            serverLink = link;
            this.client = client;
            thread = new Thread(this.serve);
        }

        public void serve()
        {
            while (client.Connected)
            {
                Packet Complete_message = ReadMessage();
                HandleMessage(Complete_message);
            }
        }

        public void start()
        {
            
            thread.Start();
           // thread.Join();

        }

        private Packet ReadMessage()
        {
            Packet receipt = serverLink.ReadChannel();
            string full_message = receipt.Content;
            while (receipt.Content[receipt.Content.Length - 1] != '\n')
            {
                receipt = serverLink.ReadChannel();
                full_message += receipt.Content;
            }
            full_message = full_message.Remove(full_message.Length - 1);
            Packet full_reciept = new Packet(receipt.SrcIp, receipt.DestIp, full_message);
            return full_reciept;
        }

        private void HandleMessage(Packet reciept)
        {
            serverLink.WriteMessage(reciept.Content, IPAddress.Loopback);
        }
    }
}
