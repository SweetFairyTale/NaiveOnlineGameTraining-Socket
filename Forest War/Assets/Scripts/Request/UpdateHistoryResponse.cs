using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class UpdateHistoryResponse : BaseRequest
{
    private RoomListPanel roomListPanel;
    private int totalCount;
    private int winCount;
    private bool isUpdateHistory = false;

    public override void Awake()
    {
        actionCode = ActionCode.UpdateHistory;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    void Update()
    {
        if(isUpdateHistory)
        {
            roomListPanel.OnUpdateHistoryResponse(totalCount, winCount);
            isUpdateHistory = false;
        }
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        totalCount = int.Parse(strs[0]);
        winCount = int.Parse(strs[1]);
        isUpdateHistory = true;
    }
}
