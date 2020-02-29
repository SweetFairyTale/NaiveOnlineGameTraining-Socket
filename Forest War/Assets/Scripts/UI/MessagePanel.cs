using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{
    private Text msgText;
    private readonly float showTime = 1.5f;
    private string message = null;

    private void Update()
    {
        if(message != null)
        {
            ShowMessage(message);
            message = null;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        msgText = GetComponent<Text>();
        msgText.enabled = false;
        uiManager.InitMessagePanel(this);  //BasePanel继承自MonoBehaviour，不能直接在UIManager中new信息面板对象.
    }

    public void ShowMessageAsync(string msg)
    {
        message = msg;
    }

    public void ShowMessage(string msg)
    {
        //msgText.color = new Color(0.85f, 0.85f, 0.85f, 1);
        msgText.CrossFadeAlpha(1, 0.3f, false);
        msgText.text = msg;
        msgText.enabled = true;
        Invoke("Hide", showTime);
    }

    private void Hide()
    {
        msgText.CrossFadeAlpha(0, 0.6f, false);
    }
}
