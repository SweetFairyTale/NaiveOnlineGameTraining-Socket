using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.MyServer
{
    enum RoomState
    {
        WaitingJoin,
        WaitingPrepare,
        Battle,
        End
    }

    class Room
    {
        private List<Client> clientRoom = new List<Client>();
        private RoomState state = RoomState.WaitingJoin;
    }
}
