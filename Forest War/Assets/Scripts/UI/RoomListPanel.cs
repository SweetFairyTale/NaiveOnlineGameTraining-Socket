using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    private RectTransform personalInfo;
    private RectTransform roomList;
    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPrefab;

    //void Start()  //涉及此处两个组件的动画在Start方法初始化前就被使用，会报空引用.
    //{
    //    personalInfo = transform.Find("PersonalInfo").GetComponent<RectTransform>();
    //    roomList = transform.Find("RoomList").GetComponent<RectTransform>();
    //}

    //void Start()  //test
    //{
    //    personalInfo = transform.Find("PersonalInfo").GetComponent<RectTransform>();
    //    roomList = transform.Find("RoomList").GetComponent<RectTransform>();
    //    roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
    //    roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;
    //}

    public override void OnEnter()
    {
        
        personalInfo = transform.Find("PersonalInfo").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;
        EnterAnim();

        SetBattleHistory();  //!! 调用时机
    }

    
    public override void OnPause()
    {
        ExitAnim(() => gameObject.SetActive(false));
    }

    public override void OnResume()
    {
        EnterAnim();
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public void OnCreateRoomButtonClick()
    {
        uiManager.PushPanel(UIPanelType.CreateRoom);
    }

    public void OnCloseButtonClick()
    {
        PlayClickSound();
        ExitAnim(() => uiManager.PopPanel());
    }

    private void EnterAnim()
    {
        gameObject.SetActive(true);
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

    private void SetBattleHistory()
    {
        UserData userData = GameFacade.Instance.GetUserData();

        transform.Find("PersonalInfo/Username").GetComponent<Text>().text = userData.Username;
        transform.Find("PersonalInfo/TotalCount").GetComponent<Text>().text = "总场数：" + userData.TotalCount.ToString();
        transform.Find("PersonalInfo/WinCount").GetComponent<Text>().text = "胜场数：" + userData.WinCount.ToString();
    }

    private void LoadRoomItem(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject roomItem = Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(roomLayout.transform);
        }

        int roomItemCount = GetComponentsInChildren<RoomItem>().Length;
        roomLayout.GetComponent<RectTransform>().sizeDelta 
            = new Vector2(roomLayout.GetComponent<RectTransform>().sizeDelta.x, 
            roomItemCount * roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing);
    }

    //void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        LoadRoomItem(1);
    //    }
    //}
}
