using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetworkLib;

namespace VSY_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 13000);
            Dictionary<string,TcpClient> clientList = new Dictionary<string,TcpClient>();
            try
            {
                while (true)
                {
                    ServerLink serverLink = new ServerLink(server, clientList);
                    serverLink.Listen();
                    ServeThread serveThread = new ServeThread(serverLink);
                    serveThread.start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                // server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
