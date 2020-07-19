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
    //protected GameFacade facade;

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

    public virtual void SendRequest()
    {

    }

    public virtual void OnResponse(string data)
    {

    }

    public virtual void OnDestroy()
    {
        if (GameFacade.Instance != null)
            GameFacade.Instance.RemoveRequest(actionCode);
    }

}
