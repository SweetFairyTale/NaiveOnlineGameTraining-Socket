using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class StartGameRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartGame;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("default");
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        if(returnCode == ReturnCode.Success)
        {
            roomPanel.OnStartSuccessResponse();
        }
        else if(returnCode == ReturnCode.Fail)
        {
            roomPanel.OnStartFailResponse();
        }
    }
}
