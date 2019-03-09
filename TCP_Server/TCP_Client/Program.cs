using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TCP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2049));

            byte[] data = new byte[1024];
            int count = clientSocket.Receive(data);
            string msg = Encoding.UTF8.GetString(data, 0, count);
            Console.WriteLine(msg);
            
            while(true)  //test
            {
                string str = Console.ReadLine();
                if(str == "c")
                {
                    clientSocket.Close();
                    return;
                }
                clientSocket.Send(Encoding.UTF8.GetBytes(str));
            }

            Console.ReadKey();
            clientSocket.Close();

        }
    }
}
