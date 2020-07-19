using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ListRoomRequest : BaseRequest
{
    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("default");
    }

    public override void OnResponse(string data)  //△很不好看的方法 要改的
    {
        List<UserData> userDateList = new List<UserData>();
        if (data != Constant.NO_WAITING_ROOM)
        {
            string[] udArray = data.Split('|');
            foreach (string ud in udArray)
            {
                string[] strs = ud.Split(',');
                userDateList.Add(new UserData(int.Parse(strs[0]), strs[1], int.Parse(strs[2]), int.Parse(strs[3])));
            }
        }       
        roomListPanel.LoadRoomItemsAsync(userDateList);       
    }
}
