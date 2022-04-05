using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HiloCodeChallenge
{
    public static class TCPServer
    {
        private const int CONNECTION_DURATION = 5000;//In milliseconds

        private static bool running = false;
        private static TcpListener server = null;
        private static Int32 port = -1;
        private static IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        private static Dictionary<TcpClient, DateTime> tcpClientsConnections;
        private static Thread thread;

        public static void Start(int portNumber)
        {
            if (portNumber < 1 || portNumber > 65535)
            {
                throw new ArgumentException("Invalid port number.");
            }
            port = portNumber;
            server = new TcpListener(localAddr, port);
            tcpClientsConnections = new Dictionary<TcpClient, DateTime>();

            thread = new Thread(TCPServer.Run);
            thread.Start();
        }

        public static void Close()
        {
            running = false;
            if (server != null)
            {
                server.Stop();
            }
            if (thread != null)
            {
                thread.Join();
            }
        }

        private static void Run()
        {
            running = true;
            server.Start();
            ListenForClients();
        }

        private static void ListenForClients()
        {
            while (running)
            {
                try
                {
                    //blocks until a client has connected to the server
                    TcpClient client = server.AcceptTcpClient();
                    tcpClientsConnections.Add(client, DateTime.Now);

                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommunications));
                    clientThread.Start(client);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void HandleClientCommunications(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            Byte[] bytes = new Byte[256];
            String data = null;

            while (running)
            {
                try
                {
                    if ((DateTime.Now - tcpClientsConnections[tcpClient]).TotalMilliseconds >= CONNECTION_DURATION)
                    {
                        tcpClient.Close();
                        tcpClientsConnections.Remove(tcpClient);
                        break;
                    }

                    int i;
                    while (clientStream.DataAvailable && (i = clientStream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        tcpClientsConnections[tcpClient] = DateTime.Now;

                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        var output = Commands.Execute(data);

                        byte[] msg = Encoding.ASCII.GetBytes(output);
                        clientStream.Write(msg, 0, msg.Length);
                    }

                    //Short wait to prevent actively waiting.
                    Task.Delay(10).Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
