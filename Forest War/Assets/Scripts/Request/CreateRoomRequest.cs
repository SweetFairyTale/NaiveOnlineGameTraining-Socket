using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CreateRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    public void SetRoomPanel(BasePanel panel)
    {
        roomPanel = panel as RoomPanel;
    }

    public override void SendRequest()
    {
        base.SendRequest("default");  //创建房间请求不需要发送数据，此处传值防止服务器解析出错.
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        RoleType roleType = (RoleType)int.Parse(strs[1]);
        GameFacade.Instance.SetCurrentRoleType(roleType);
        if(returnCode == ReturnCode.Success)
        {
            roomPanel.SetHostPlayerInfoAsync();
        }
    }
}
