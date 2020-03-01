using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomListPanel : BasePanel
{
    private RectTransform personalInfo;
    private RectTransform roomList;

    //void Start()  //涉及此处两个组件的动画在Start方法初始化前就被使用，会报空引用.
    //{
    //    personalInfo = transform.Find("PersonalInfo").GetComponent<RectTransform>();
    //    roomList = transform.Find("RoomList").GetComponent<RectTransform>();
    //}

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        personalInfo = transform.Find("PersonalInfo").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        EnterAnim();
    }

    
    public override void OnPause()
    {
        ExitAnim(() => gameObject.SetActive(false));
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public void OnCloseButtonClick()
    {
        PlayClickSound();
        ExitAnim(() => uiManager.PopPanel());
    }

    private void EnterAnim()
    {
        personalInfo.localPosition = new Vector3(-1000, 0);
        personalInfo.DOLocalMoveX(-240, 0.5f);

        roomList.localPosition = new Vector3(1000, 0);
        roomList.DOLocalMoveX(90, 0.5f);
    }

    private void ExitAnim(TweenCallback action)
    {
        personalInfo.DOLocalMoveX(-1000, 0.5f);
        roomList.DOLocalMoveX(1000, 0.5f).OnComplete(action);
    }
}
