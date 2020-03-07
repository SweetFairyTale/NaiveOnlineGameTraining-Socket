using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class LoginRequest : BaseRequest
{
    private LoginPanel loginPanel;  //request类无法直接调用uiManager上的方法，必须持有面板的引用.

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }

    public void SendRequest(string username, string password)
    {
        string data = username + ',' + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]); 
        loginPanel.OnLoginResponse(returnCode);  //MAYDO 在Panel中return true并在此处处理进入房间.
        if(returnCode == ReturnCode.Success)
        {
            string username = strs[1];
            int totalCount = int.Parse(strs[2]);
            int winCount = int.Parse(strs[3]);
            UserData ud = new UserData(username, totalCount, winCount);
            GameFacade.Instance.SetUserData(ud);
        }
    }
}
