using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SyncRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.SyncRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        if (data == Constant.OTHERS_QUIT_ROOM)
        {
            roomPanel.ClearOtherPlayerAsync();
        }
        else
        {
            UserData other = new UserData(data);
            roomPanel.SetOtherPlayerInfoAsync(other);
        }
    }
}
