using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GameServer.Controller;
namespace GameServer.Server
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;  //创建服务器端Socket.
        private List<Client> clientList;  //管理所有客户端.

        //其他类通过服务器上的controllerManager对象处理消息，减少耦合.
        private ControllerManager controllerManager = new ControllerManager();

        public Server(){ }

        public Server(string ipStr, int port)
        {
            SetIpAndPort(ipStr, port);
        }

        public void SetIpAndPort(string ipStr, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);  //绑定ip端口.
            serverSocket.Listen(0);  //监听 不设限.
            serverSocket.BeginAccept(AcceptCallBack, null);  //异步 可接收多个连接.

        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client);
        }

        public void RemoveClient(Client client)
        {
            lock(clientList)
            {
                clientList.Remove(client);
            }
        }

    }
}
