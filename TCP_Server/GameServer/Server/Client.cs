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
        private Server server;  //持有服务器引用.
        private Message msg = new Message();  //客户端使用Message类处理消息.

        public Client() { }
        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
        }

        public void Start()
        {
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
            //?
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                //在回调函数中接收数据.
                int count = clientSocket.EndReceive(ar);  //新消息的数量.
                if (count == 0)
                {
                    Close();
                }
                //TODO 处理接收到的数据.
                msg.ReadMessage(count);  //读取count数量的消息.
                Start();
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
