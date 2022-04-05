using System;
using System.Linq;

namespace HiloCodeChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 1)
            {
                Console.WriteLine("Specify a port number for the servers to use.");
                Environment.Exit(1);
            }

            int port = -1;
            if(!int.TryParse(args[0], out port))
            {
                Console.WriteLine("The specified port number is not a number.");
                Environment.Exit(1);
            }
            if (port < 1 || port > 65535)
            {
                Console.WriteLine("The specified port number for the servers is invalid. Should be between 1 and 65535.");
                Environment.Exit(1);
            }

            try
            {
                TCPServer.Start(port);
                UDPServer.Start(port);

                Console.WriteLine("Press ENTER to close the servers.");
                Console.ReadLine();
            }
            finally
            {
                TCPServer.Close();
                UDPServer.Close();
            }
        }
    }
}
