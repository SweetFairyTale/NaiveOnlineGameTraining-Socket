using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GoPlayResponse : BaseRequest
{
    private bool isInitGame = false;

    public override void Awake()
    {
        actionCode = ActionCode.GoPlay;        
        base.Awake();
    }

    void Update()
    {
        if(isInitGame)
        {
            GameFacade.Instance.InitGame();
            isInitGame = false;
        }
    }

    public override void OnResponse(string data)
    {
        isInitGame = true;
    }
}
