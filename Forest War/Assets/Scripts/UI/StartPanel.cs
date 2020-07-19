using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel
{
    private Button loginButton;

    void Start()
    {
        loginButton = GetComponentInChildren<Button>();
        loginButton.onClick.AddListener(OnLoginButtonClick);
    }

    private void OnLoginButtonClick()
    {
        PlayClickSound();
        loginButton.GetComponent<Animator>().enabled = false;
        uiManager.PushPanel(UIPanelType.Login);
    }

    public override void OnPause()
    {
        loginButton.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public override void OnResume()
    {
        gameObject.SetActive(true);
        loginButton.transform.DOScale(1, 0.2f).OnComplete(()=>
            loginButton.GetComponent<Animator>().enabled = true
        );
    }

}
