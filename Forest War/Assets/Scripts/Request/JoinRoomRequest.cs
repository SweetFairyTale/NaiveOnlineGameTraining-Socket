using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class JoinRoomRequest : BaseRequest
{
    private RoomListPanel roomListPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public void SendRequest(int uid)
    {
        base.SendRequest(uid.ToString());
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('-');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
      
        UserData host = null;
        UserData other = null;
        if(returnCode == ReturnCode.Success)
        {
            RoleType roleType = (RoleType)int.Parse(strs[1]);
            GameFacade.Instance.SetCurrentRoleType(roleType); //不需要使用facade异步设置roletype到PlayManager

            string[] udStrArray = strs[2].Split('|');
            host = new UserData(udStrArray[0]);
            other = new UserData(udStrArray[1]);          
        }
        roomListPanel.OnJoinServerResponse(returnCode, host, other);
    }

}
