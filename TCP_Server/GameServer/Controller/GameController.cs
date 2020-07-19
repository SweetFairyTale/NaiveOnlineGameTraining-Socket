using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.MyServer;

namespace GameServer.Controller
{
    class GameController : BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }

        public string StartGame(string data, Client client, Server server)
        {
            if(client.IsHost() && client.Room.IsWaitingBattle())
            {
                Room room = client.Room;
                room.BroadcastMessageToOthers(client, ActionCode.StartGame, ((int)ReturnCode.Success).ToString());
                room.StartCountdown();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        public string SyncMove(string data, Client client, Server server)
        {
            Room room = client.Room;
            if(room != null)
                room.BroadcastMessageToOthers(client, ActionCode.SyncMove, data);
            return null;
        }

        public string SyncArrow(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessageToOthers(client, ActionCode.SyncArrow, data);
            return null;
        }

        public string CauseDamage(string data, Client client, Server server)
        {
            int damage = int.Parse(data);
            Room room = client.Room;
            if (room == null)
                return null;
            room.TakeDamage(damage, client);
            return null;
        }

        public string AbortGame(string data, Client client, Server server)
        {
            Room room = client.Room;
            if(room != null)
            {
                room.BroadcastMessageToOthers(null, ActionCode.AbortGame, "default");
                room.Close();
            }
            return null;
        }
    }
}
