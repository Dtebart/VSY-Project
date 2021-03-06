﻿using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class ClientLink : Link
    {
        // ---------------------- Properties ----------------------
        public IClient _iClient;
        private Thread _readThread;
        private IPAddress[] _IPserver;

        // ---------------------- Constructors ----------------------

        public ClientLink(IClient client)
        {
            _port = 13000;
            _IPserver = ReadServerlist("serverlist.txt");
            Connect();
            _iClient = client;

            StartReading();
        }

        public ClientLink(IClient client, int port)
        {
            _port = port;
            _iClient = client;
            StartReading();
        }

        // ---------------------- Functions ----------------------
        private IPAddress[] ReadServerlist(String fileName)
        {
            List<IPAddress> serverList = new List<IPAddress>();
            try
            {
                StreamReader sr = new StreamReader(fileName);
                while (!sr.EndOfStream)
                {
                    String serverIp = sr.ReadLine();
                    serverList.Add(IPAddress.Parse(serverIp));
                }

                return serverList.ToArray();
            }
            catch (Exception e)
            {
                // Do something here
                return null;
            }
        }
        private void StartReading()
        {
            _stream = _client.GetStream();

            // Start thread for reading data
            ThreadStart readDel = new ThreadStart(Serve);
            _readThread = new Thread(readDel);
            _readThread.SetApartmentState(ApartmentState.STA);

            _readThread.Start();
        }

        public void Serve()
        {
            while (_client.Connected)
            {
                try
                {
                    Packet recievedPacket = ReadChannel();
                    _iClient.ActionAfterRead(recievedPacket);
                }
                catch (System.IO.IOException e)
                {
                    _iClient.Disconnected();
                }

            }
        }

        public void Close()
        {
            _readThread.Abort();
            _client.Close();
        }

        public static IPAddress GetLocalIP()
        {
            IPHostEntry host;
            String localIP = String.Empty;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {

                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return IPAddress.Parse(localIP);
        }


        public void Connect(int ServerNumber = 0)
        {
            IPAddress IPclient = GetLocalIP();
            IPEndPoint serverEndPoint = new IPEndPoint(_IPserver[ServerNumber], 13000);
            IPEndPoint clientEndPoint = new IPEndPoint(IPclient, 0);
            _client = new TcpClient(clientEndPoint);
            IAsyncResult result = _client.BeginConnect(_IPserver[ServerNumber], 13000, null, null);
            Boolean success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

            if (!success)
            {
                if (_IPserver.Length > ServerNumber)
                    Connect(ServerNumber + 1);
            }
        }
    }
}
