using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HiloCodeChallenge
{
    public static class UDPServer
    {
        private static bool running = false;
        private static int port = -1;
        private static UdpClient listener;
        private static IPEndPoint groupEP;
        private static Thread thread;

        public static void Start(int portNumber)
        {
            if (portNumber < 1 || portNumber > 65535)
            {
                throw new ArgumentException("Invalid port number.");
            }

            port = portNumber;

            listener = new UdpClient(port);
            groupEP = new IPEndPoint(IPAddress.Any, port);

            thread = new Thread(UDPServer.Run);
            thread.Start();
        }

        public static void Close()
        {
            running = false;
            if (listener != null)
            {
                listener.Close();
            }
            if (thread != null)
            {
                thread.Join();
            }
        }

        private static void Run()
        {
            running = true;
            while (running)
            {
                try
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    string output = Commands.Execute(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                    byte[] msg = Encoding.ASCII.GetBytes(output);
                    listener.Send(msg, msg.Length, groupEP);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
