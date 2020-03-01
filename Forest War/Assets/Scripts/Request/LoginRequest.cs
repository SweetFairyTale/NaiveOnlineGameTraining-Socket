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
        ReturnCode returnCode = (ReturnCode)int.Parse(data);      
        loginPanel.OnLoginResponse(returnCode);  //MAYDO 在Panel中return true并在此处处理进入房间.
        //Debug.Log("ReturnCode:" + returnCode.ToString());
    }
}
