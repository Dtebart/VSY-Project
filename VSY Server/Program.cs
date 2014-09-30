using System;
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
            try
            {
                ServerLink serverLink = new ServerLink();

                while (true)
                {
                    TcpClient client = serverLink.Listen();
                    Packet receipt = serverLink.ReadChannel();
                    serverLink.WriteMessage(receipt.Content, IPAddress.Loopback);
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
