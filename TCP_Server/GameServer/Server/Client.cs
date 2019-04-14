using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace GameServer.Server
{
    class Client
    {
        private Socket clientSocket;  //创建客户端Socket.
        private Server server; //持有服务器引用.

        public Client() { }
        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
        }

        public void Start()
        {
            clientSocket.BeginReceive(null, 0, 0, SocketFlags.None, ReceiveCallback, null);
            //?
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                //在回调函数中接收数据.
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }
                //TODO 处理接收到的数据.
                clientSocket.BeginReceive(null, 0, 0, SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }           
        }

        private void Close()
        {
            if(clientSocket != null)
            {
                clientSocket.Close();
            }
            server.RemoveClient(this);
        }
   
    }
}
