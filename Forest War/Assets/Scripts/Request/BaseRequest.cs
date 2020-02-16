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
    private RequestCode requestCode = RequestCode.None;

    public virtual void Awake()
    {
        GameFacade.Instance.AddRequest(requestCode, this);
    }

    public virtual void SendRequest()
    {

    }

    public virtual void SendResponse(string data)
    {

    }

    public virtual void OnDestroy()
    {
        GameFacade.Instance.RemoveRequest(requestCode);
    }

}
