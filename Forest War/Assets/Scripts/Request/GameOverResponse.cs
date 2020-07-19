using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GameOverResponse : BaseRequest
{
    private GamePanel gamePanel;
    private bool isGameOver = false;
    private ReturnCode returnCode;

    public override void Awake()
    {
        actionCode = ActionCode.GameOver;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    void Update()
    {
        if(isGameOver)
        {
            if (returnCode == ReturnCode.Success)
            {
                gamePanel.OnGameVectoryResponse();
            }
            else if(returnCode == ReturnCode.Fail)
            {
                gamePanel.OnGameDefeatResponse();
            }
            isGameOver = false;
        }
    }

    public override void OnResponse(string data)
    {
        returnCode = (ReturnCode)int.Parse(data);
        isGameOver = true;
    }
}
