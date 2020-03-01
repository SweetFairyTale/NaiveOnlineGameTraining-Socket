using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  //Dotween插件.
using Common;

public class LoginPanel : BasePanel
{
    //private Button closeButton;  //用于AddListener的，未启用
    private InputField usernameInput;
    private InputField passwordInput;
    //private Button loginButton;
    //private Button registerButton;
    private LoginRequest loginRequest;

    private void Start()
    {
        //closeButton = transform.Find("CloseButton").GetComponent<Button>();        
        usernameInput = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordInput = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        //loginButton = transform.Find("LoginButton").GetComponent<Button>();
        //registerButton = transform.Find("RegisterButton").GetComponent<Button>();
        loginRequest = GetComponent<LoginRequest>();
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.3f);
        transform.localPosition = new Vector3(0, -400, 0);
        transform.DOLocalMove(Vector3.zero, 0.3f);

        //closeButton.onClick.AddListener(OnCloseButtonClick);  //由于Login页面存在多次出入栈的情况，在OnEnter中会添加多个监听.
    }

    public void OnCloseButtonClick()
    {
        PlayClickSound();
        transform.DOScale(0, 0.5f);
        transform.DOLocalMove(new Vector3(0, -400, 0), 0.3f).OnComplete(() => uiManager.PopPanel());
    }

    public void OnLoginButtonClick()
    {
        PlayClickSound();
        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            uiManager.ShowMessage("用户名或密码不能为空");
        }
        else
        {
            //通过LoginRequest发送登录数据到服务器端.
            //loginRequest.SendRequest(usernameInput.text, passwordInput.text);

            uiManager.PushPanel(UIPanelType.RoomList);
        }
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            //uiManager.ShowMessageAsync("登录成功");
            //TODO 登录成功 进入房间列表.  (或者输出消息后返回到request类中处理房间)
            uiManager.PushPanelAsync(UIPanelType.RoomList);
        }
        else
        {
            uiManager.ShowMessageAsync("用户名或密码错误");
        }

    }

    public void OnRegisterButtonClick()
    {
        PlayClickSound();
        uiManager.PushPanel(UIPanelType.Register);
    }

    public override void OnPause()
    {
        transform.DOScale(0, 0.5f);
        transform.DOLocalMove(new Vector3(0, -400, 0), 0.3f).OnComplete(() => gameObject.SetActive(false));
    }

    public override void OnResume()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(0, -400, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

}
