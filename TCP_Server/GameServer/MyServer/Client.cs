using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Common;
using MySql.Data.MySqlClient;
using GameServer.Tool;
using GameServer.Model;
using GameServer.DAO;

namespace GameServer.MyServer
{
    class Client
    {
        private Socket clientSocket;
        private Server server;  //持有服务器引用.
        private Message msg = new Message();  //客户端使用Message类处理消息.
        private MySqlConnection mysqlConn;  //客户端持有数据库连接对象引用，可供Controller调用执行数据库操作.

        private Room room;
        private User user;
        private History history;
        public int HP
        {
            get;set;
        }
        public MySqlConnection MysqlConn
        {
            get { return mysqlConn; }
        }
        public Room Room
        {
            get { return room; }
            set { room = value;  }
        }

        private HistoryDAO historyDAO = new HistoryDAO();

        public void TakeDamage(int damage)
        {
            HP -= damage;
            HP = Math.Max(HP, 0);
        }

        public bool IsDefeated()
        {
            return HP == 0;
        }

        public void SetUserData(User user, History history)
        {
            this.user = user;
            this.history = history;
        }

        public string GetUserData()
        {
            return user.Id + "," + user.Username + "," + history.TotalCount + "," + history.WinCount;
        }

        public int GetUserId()
        {
            return user.Id;
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
                if (clientSocket == null || clientSocket.Connected == false) return;
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

        public void Send(ActionCode actionCode, string data)
        {
            try
            {
                byte[] responseBytes = Message.PackResponseData(actionCode, data);
                clientSocket.Send(responseBytes);
            }
            catch(Exception e)
            {
                Console.WriteLine("[WARNNING]:Cannot Send Data To Client: " + e);
            }
        }

        public bool IsHost()
        {
            return room.IsHost(this);
        }

        private void Close()
        {
            ConnHelper.CloseConnection(mysqlConn);

            if (clientSocket != null)
                clientSocket.Close();

            if (room != null)
                room.Close(this);

            server.RemoveClient(this);
        }

        public void UpdatePlayerHistory(bool isVictory)
        {
            UpdatePlayerHistoryToDB(isVictory);
            UpdatePlayerHistoryToClient();
        }

        private void UpdatePlayerHistoryToDB(bool isVictory)
        {
            history.TotalCount++;
            if (isVictory)
            {
                history.WinCount++;
            }
            historyDAO.NewOrUpdateHistory(mysqlConn, history);
        }

        private void UpdatePlayerHistoryToClient()
        {
            Send(ActionCode.UpdateHistory, string.Format("{0},{1}", history.TotalCount, history.WinCount));
        }
    }
}
