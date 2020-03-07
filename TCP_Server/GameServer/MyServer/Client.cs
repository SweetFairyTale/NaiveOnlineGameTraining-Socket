using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Common;
using MySql.Data.MySqlClient;
using GameServer.Tool;

namespace GameServer.MyServer
{
    class Client
    {
        private Socket clientSocket;  //创建客户端Socket.
        private Server server;  //持有服务器引用.
        private Message msg = new Message();  //客户端使用Message类处理消息.
        private MySqlConnection mysqlConn;  //客户端持有数据库连接对象引用，可供UserController调用执行数据库操作.
        public MySqlConnection MysqlConn
        {
            get { return mysqlConn; }
        }

        public Client() { }

        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            mysqlConn = ConnHelper.Connect();
        }

        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);           
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
                //接收解析出的数据并转发给ControllerManager(通过自身提供给Message类的回调函数获得数据).
                msg.ReadMessage(count, OnProcessMessageCallback);  //读取count数量的消息，并为Message类提供回调方法.
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }           
        }

        private void OnProcessMessageCallback(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandleRequest(requestCode, actionCode, data, this);
        }

        private void Close()
        {
            ConnHelper.CloseConnection(mysqlConn);
            if(clientSocket != null)
            {
                clientSocket.Close();
            }
            server.RemoveClient(this);
        }

        public void Send(ActionCode actionCode, string data)
        {
            byte[] responseBytes = Message.PackResponseData(actionCode, data);
            clientSocket.Send(responseBytes);
        }
   
    }
}
