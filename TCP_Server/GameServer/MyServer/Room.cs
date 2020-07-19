using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Threading;

namespace GameServer.MyServer
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }

    class Room
    {
        private List<Client> clientsInThisRoom = new List<Client>();  //[0]=host, [1]=other
        private RoomState state = RoomState.WaitingJoin;
        private Server server;

        public Room(Server server)
        {
            this.server = server;
        }

        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }

        public bool IsWaitingBattle()
        {
            return state == RoomState.WaitingBattle;
        }

        public void AddClient(Client client)
        {
            client.HP = Constant.FULL_HP;
            clientsInThisRoom.Add(client);
            client.Room = this;
            if(clientsInThisRoom.Count >= 2)
            {
                state = RoomState.WaitingBattle;
            }
        }

        public void RemoveTheOtherClient()
        {
            if(clientsInThisRoom != null)
            {
                clientsInThisRoom[1].Room = null;
                clientsInThisRoom.Remove(clientsInThisRoom[1]);
                state = RoomState.WaitingJoin;
            }          
        }

        //通过Client类中的方法获取用户信息，用于展示房间列表
        //return "id,username,totoalCount,winCount"
        public string GetHostData()
        {
            if(clientsInThisRoom[0] != null)
                return clientsInThisRoom[0].GetUserData();
            return "-,-,-,-";
        }

        public string GetTheOtherData()
        {
            if (clientsInThisRoom[1] != null)
                return clientsInThisRoom[1].GetUserData();
            return "-,-,-,-";
        }

        public int GetHostId()
        {
            if(clientsInThisRoom.Count > 0)
            {
                return clientsInThisRoom[0].GetUserId();
            }
            return -1;
        }

        //其他用户加入房间时将信息同步给房主
        public void OnOtherClientJoin(string data)
        {
            server.SendResponse(clientsInThisRoom[0], ActionCode.SyncRoom, data);
        }

        //其他用户退出房间时告知房主
        public void OnOtherClientQuit()
        {
            server.SendResponse(clientsInThisRoom[0], ActionCode.SyncRoom, Constant.OTHERS_QUIT_ROOM);
        }

        //△扩展方法
        public void BroadcastMessageToOthers(Client excludeClient, ActionCode actionCode, string data)
        {
            foreach(Client client in clientsInThisRoom)
            {
                if(client != excludeClient)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
        }

        public bool IsHost(Client client)
        {
            return client == clientsInThisRoom[0];
        }

        //指定客户端退出
        public void Close(Client client)
        {
            client.Room = null;
            if (client == clientsInThisRoom[0])
            {
                //server.RemoveRoom(this);
                Close();
            }
            else
            {
                clientsInThisRoom.Remove(client);
            }              
        }

        //删除所有用户的房间引用
        public void Close()
        {
            foreach(Client client in clientsInThisRoom)
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
        }

        public void StartCountdown()
        {
            new Thread(CountdownThread).Start();
        }

        //服务器上倒计时321并通知到客户端.
        private void CountdownThread()
        {
            Thread.Sleep(300);
            for (int i = 3; i >0; i--)
            {
                BroadcastMessageToOthers(null, ActionCode.Countdown, i.ToString());
                Thread.Sleep(1000);
            }
            BroadcastMessageToOthers(null, ActionCode.GoPlay, Constant.NO_DATA_REQUEST);
        }

        public void TakeDamage(int damage, Client excludeClient)
        {
            bool GameOver = false;
            foreach(Client client in clientsInThisRoom)
            {
                if(client != excludeClient)
                {
                    client.TakeDamage(damage);
                    if (client.IsDefeated())
                    {
                        GameOver = true;  //其中一个角色被击败后结束游戏.
                    }
                }
            }            

            //处理游戏结束
            if (!GameOver)
                return;
            else
                this.GameOver();
        }

        private void GameOver()
        {
            foreach (Client client in clientsInThisRoom)
            {
                if (client.IsDefeated())
                {
                    client.UpdatePlayerHistory(false);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                }
                else
                {
                    client.UpdatePlayerHistory(true);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                }
            }
            Close();
        }
    }
}
