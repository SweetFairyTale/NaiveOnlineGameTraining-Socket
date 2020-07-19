using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class RegisterPanel : BasePanel
{
    private InputField usernameInput;
    private InputField passwordInput;
    private InputField repasswordInput;
    private RegisterRequest registerRequest;

    private void Start()
    {
        usernameInput = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordInput = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        repasswordInput = transform.Find("RepasswordLabel/RepasswordInput").GetComponent<InputField>();
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterButtonClick);
        registerRequest = GetComponent<RegisterRequest>();
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.3f);
        transform.localPosition = new Vector3(0, -400, 0);
        transform.DOLocalMove(Vector3.zero, 0.3f);
    }

    public void OnCloseButtonClick()
    {
        PlayClickSound();
        transform.DOScale(0, 0.5f);
        transform.DOLocalMove(new Vector3(0, -400, 0), 0.2f).OnComplete(
            () => uiManager.PopPanel()
            );
    }

    public void OnRegisterButtonClick()
    {
        PlayClickSound();
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            uiManager.ShowMessage("用户名不能为空");
        }
        else if(string.IsNullOrEmpty(passwordInput.text))
        {
            uiManager.ShowMessage("密码不能为空");
        }
        else if(passwordInput.text != repasswordInput.text)
        {
            uiManager.ShowMessage("两次输入密码不一致");
        }
        else
        {            
            //允许发起注册请求.
            registerRequest.SendRequest(usernameInput.text, passwordInput.text);
        }
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if(returnCode == ReturnCode.Success)
        {
            uiManager.ShowMessageAsync("注册成功");
        }
        else
        {
            uiManager.ShowMessageAsync("用户名重复");
        }
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
}
