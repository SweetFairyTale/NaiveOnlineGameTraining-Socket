using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GameServer.Controller;
using Common;

namespace GameServer.MyServer
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;  //服务器端Socket.
        private List<Client> clientList;  //管理所有客户端.

        //其他类通过服务器上的controllerManager对象处理消息，减少耦合.
        private ControllerManager controllerManager;

        public Server(){ }

        public Server(string ipStr, int port)
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr, port);
        }

        public void SetIpAndPort(string ipStr, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        //关于Socket的初始化.
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);  //绑定ip端口.
            serverSocket.Listen(0);  //监听 连接数不设限.
            serverSocket.BeginAccept(AcceptCallBack, null);  //阻塞并等待客户端连接.
         
        }

        //回调方法，创建一个客户端连接并加入列表.
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

        /// <summary>
        /// 返回数据结果以响应客户端.
        /// </summary>
        /// <param name="client">指定客户端</param>
        /// <param name="requestCode">指定处理类型</param>
        /// <param name="data"></param>
        public void SendResponse(Client client, RequestCode requestCode, string data)  //在ControllerManager中调用.
        {
            client.Send(requestCode, data);
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
    }
}
