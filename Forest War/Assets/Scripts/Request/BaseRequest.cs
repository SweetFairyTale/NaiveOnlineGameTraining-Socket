using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

/// <summary>
/// 用于派生所有的request类
/// 通过GameFacade的单例与RequestManager交互
/// </summary>
public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    //protected GameFacade facade;  //好像也没什么卵用(


    public virtual void Awake()
    {
        GameFacade.Instance.AddRequest(actionCode, this);
        //facade = GameFacade.Instance;
    }

    protected void SendRequest(string data)
    {
        GameFacade.Instance.SendRequest(requestCode, actionCode, data);
        //facade.SendRequest(requestCode, actionCode, data);
    }

    //暂时好像没什么卵用//
    public virtual void SendRequest()
    {

    }

    public virtual void OnResponse(string data)
    {

    }

    public virtual void OnDestroy()
    {
        GameFacade.Instance.RemoveRequest(actionCode);
    }

}
