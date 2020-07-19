using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.MyServer;

namespace GameServer.Controller
{
    class RoomController : BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }

        public string CreateRoom(string data, Client client, Server server)
        {
            server.CreateRoom(client);
            return ((int)ReturnCode.Success).ToString() + "," + ((int)RoleType.Blue).ToString();
        }

        public string ListRoom(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Room room in server.GetRoomList())
            {
                if(room.IsWaitingJoin())
                {
                    sb.Append(room.GetHostData() + "|");
                }
            }
            if(sb.Length == 0)
            {
                sb.Append(Constant.NO_WAITING_ROOM);
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public string JoinRoom(string data, Client client, Server server)
        {
            int uid = int.Parse(data);
            Room room = server.GetRoomById(uid);
            if(room == null)
            {
                return ((int)ReturnCode.NotFound).ToString();  //房间不存在(房主已退出但本地用户未刷新)
            }
            else if(room.IsWaitingJoin() == false)
            {
                return ((int)ReturnCode.Fail).ToString();  //房间满员
            }
            else
            {
                room.AddClient(client);       
                string twoPlayerDatas = room.GetHostData() + "|" + room.GetTheOtherData();
                room.OnOtherClientJoin(room.GetTheOtherData());
                return ((int)ReturnCode.Success).ToString() + "-" + ((int)RoleType.Red).ToString() + "-" + twoPlayerDatas; 
            }
        }

        public string QuitRoom(string data, Client client, Server server)
        {
            bool isHost = client.IsHost();
            Room room = client.Room;
            if (isHost)
            {
                room.BroadcastMessageToOthers(client, ActionCode.QuitRoom, ((int)ReturnCode.Success).ToString());
                room.Close();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                //other player quit                
                client.Room.RemoveTheOtherClient();
                room.OnOtherClientQuit();
                return ((int)ReturnCode.Success).ToString();
            }
        }
    }
}
