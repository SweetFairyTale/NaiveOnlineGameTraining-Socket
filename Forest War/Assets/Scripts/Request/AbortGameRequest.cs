using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class AbortGameRequest : BaseRequest
{
    private bool isAbort = false;
    private GamePanel gamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.AbortGame;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("default");
    }

    void Update()
    {
        if(isAbort)
        {
            gamePanel.OnAbortGameResponse();
            isAbort = false;
        }
    }

    public override void OnResponse(string data)
    {
        isAbort = true;
    }

}
