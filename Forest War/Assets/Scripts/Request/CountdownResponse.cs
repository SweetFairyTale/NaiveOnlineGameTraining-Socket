using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CountdownResponse : BaseRequest
{
    private GamePanel gamePanel;

    public override void Awake()
    {
        //不需要发送请求，无需修改requestCode
        actionCode = ActionCode.Countdown;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        int time = int.Parse(data);
        gamePanel.ShowTimeAsync(time);
    }
}
