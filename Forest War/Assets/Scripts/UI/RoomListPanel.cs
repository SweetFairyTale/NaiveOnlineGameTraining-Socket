using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

public class RoomListPanel : BasePanel
{
    private RectTransform personalInfo;
    private RectTransform roomList;
    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPrefab;
    private List<UserData> userDataList = null;

    private ListRoomRequest listRoomRequest;
    private CreateRoomRequest createRoomRequest;
    private JoinRoomRequest joinRoomRequest;

    private UserData host = null;
    private UserData other = null;

    void Awake()
    {
        personalInfo = transform.Find("PersonalInfo").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;

        listRoomRequest = GetComponent<ListRoomRequest>();
        createRoomRequest = GetComponent<CreateRoomRequest>();
        joinRoomRequest = GetComponent<JoinRoomRequest>();
    }

    public override void OnEnter()
    {         
        EnterAnim();
        SetLocalPlayerHistory();
        listRoomRequest.SendRequest();      
    }
  
    public override void OnPause()
    {
        ExitAnim(() => gameObject.SetActive(false));
    }

    public override void OnResume()
    {
        EnterAnim();
        listRoomRequest.SendRequest();  //再次发送获取列表请求以刷新页面.
    }

    public override void OnExit()
    {
        ExitAnim(() => gameObject.SetActive(false));
    }

    public void OnCreateRoomButtonClick()
    {
        PlayClickSound();
        BasePanel panel = uiManager.PushPanel(UIPanelType.Room);
        createRoomRequest.SetRoomPanel(panel);
        createRoomRequest.SendRequest();
    }

    public void OnCloseButtonClick()
    {
        PlayClickSound();
        ExitAnim(() => uiManager.PopPanel());
    }

    public void OnRefreshButtonClick()
    {
        PlayClickSound();
        listRoomRequest.SendRequest();
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

    private void SetLocalPlayerHistory()
    {
        UserData userData = GameFacade.Instance.GetUserData();

        transform.Find("PersonalInfo/Username").GetComponent<Text>().text = userData.Username;
        transform.Find("PersonalInfo/TotalCount").GetComponent<Text>().text = "总场数：" + userData.TotalCount.ToString();
        transform.Find("PersonalInfo/WinCount").GetComponent<Text>().text = "胜场数：" + userData.WinCount.ToString();
    }

    public void LoadRoomItemsAsync(List<UserData> userDataList)
    {
        this.userDataList = userDataList;
    }

    void Update()
    {
        if (userDataList != null)
        {
            LoadRoomItems(userDataList);
            userDataList = null;
        }
        if (host != null && other != null)
        {
            (uiManager.PushPanel(UIPanelType.Room) as RoomPanel).SetTwoPlayersInfo(host, other);
            host = null;
            other = null;
        }
    }

    private void LoadRoomItems(List<UserData> userDataList)
    {
        ClearCurrentRoomItems();
        for (int i = 0; i < userDataList.Count; i++)
        {
            GameObject roomItem = Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(roomLayout.transform);
            UserData userData = userDataList[i];
            roomItem.GetComponent<RoomItem>().SetRoomItem(userData.ID, userData.Username, userData.TotalCount, userData.WinCount, this);
        }

        //动态设置Layout的高度
        int roomItemCount = GetComponentsInChildren<RoomItem>().Length;
        roomLayout.GetComponent<RectTransform>().sizeDelta 
            = new Vector2(roomLayout.GetComponent<RectTransform>().sizeDelta.x, 
            roomItemCount * roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing);
    }

    private void ClearCurrentRoomItems()
    {
        RoomItem[] riArray = roomLayout.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem ri in riArray)
        {
            ri.DestroySelf();
        }
    }

    public void OnJoinButtonClick(int uid)
    {
        PlayClickSound();
        joinRoomRequest.SendRequest(uid);
    }

    public void OnJoinServerResponse(ReturnCode returnCode, UserData host, UserData other)
    {
        switch(returnCode)
        {
            case ReturnCode.NotFound:
                uiManager.ShowMessageAsync("房间不存在");
                break;
            case ReturnCode.Fail:
                uiManager.ShowMessageAsync("房间已满，无法加入");
                break;
            case ReturnCode.Success:
                this.host = host;
                this.other = other;              
                break;
        }
    }

    public void OnUpdateHistoryResponse(int totalCount, int winCount)
    {
        GameFacade.Instance.UpdateUserData(totalCount, winCount);
        SetLocalPlayerHistory();
    }
}
