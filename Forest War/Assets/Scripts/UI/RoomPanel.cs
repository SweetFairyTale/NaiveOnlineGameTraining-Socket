using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RoomPanel : BasePanel
{

    private Text localPlayerUsername;
    private Text localPlayerTotalCount;
    private Text localPlayerWinCount;

    private Text enemyPlayerUsername;
    private Text enemyPlayerTotalCount;
    private Text enemyPlayerWinCount;

    private RectTransform localPlayerPanel;
    private RectTransform enemyPlayerPanel;
    private Transform startButton;
    private Transform quitButton;

    //private UserData ud = null;
    //情形一：当前用户创建房间(host != null, other == null)，获取并显示自己的信息
    //情形二：当前用户加入其它房间(host != null, other != null)，获取并显示host和自己的信息
    //情形三：当前用户作为房主时有其他用户加入(host == null, other != null)，获取并显示他人的信息
    private UserData host = null;
    private UserData other = null;

    private QuitRoomRequest quitRoomRequest;
    private bool isOtherPlayerQuit = false;

    private StartGameRequest startGameRequest;
    
    //////Unity调用顺序:Awake->OnEnter->Start//////

    void Awake()
    {
        localPlayerUsername = transform.Find("BluePanel/Username").GetComponent<Text>();
        localPlayerTotalCount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        localPlayerWinCount = transform.Find("BluePanel/WinCount").GetComponent<Text>();

        enemyPlayerUsername = transform.Find("RedPanel/Username").GetComponent<Text>();
        enemyPlayerTotalCount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        enemyPlayerWinCount = transform.Find("RedPanel/WinCount").GetComponent<Text>();

        localPlayerPanel = transform.Find("BluePanel").GetComponent<RectTransform>();
        enemyPlayerPanel = transform.Find("RedPanel").GetComponent<RectTransform>();
        startButton = transform.Find("StartButton");
        quitButton = transform.Find("QuitButton");
    }

    void Start()
    {
        quitRoomRequest = GetComponent<QuitRoomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        EnterAnim();
    }

    public override void OnPause()
    {
        ExitAnim(() => gameObject.SetActive(false));
    }

    public override void OnExit()
    {
        ExitAnim(() => gameObject.SetActive(false));
    }

    public void OnStartButtonClick()
    {
        PlayClickSound();
        startGameRequest.SendRequest();
    }

    public void OnQuitButtonClick()
    {
        PlayClickSound();
        quitRoomRequest.SendRequest();
    }

    public void OnQuitResponse()
    {
        uiManager.PopPanelAsync();
    }

    public void OnStartSuccessResponse()
    {
        uiManager.PushPanelAsync(UIPanelType.Game);
        GameFacade.Instance.OnGameBeginAsync();
    }

    public void OnStartFailResponse()
    {
        uiManager.ShowMessageAsync("人数不满无法开始对战");
    }

    //玩家作为房主创建房间时显示自己信息.
    public void SetHostPlayerInfoAsync()
    {
        host = GameFacade.Instance.GetUserData();
    }

    //玩家作为房主并有其他玩家加入时显示他人信息.
    public void SetOtherPlayerInfoAsync(UserData other)
    {
        this.other = other;
    }

    //玩家加入他人房间时显示两个用户信息.
    public void SetTwoPlayersInfo(UserData host, UserData other)
    {
        SetHostPlayerInfo(host.Username, host.TotalCount.ToString(), host.WinCount.ToString());
        SetOtherPlayerInfo(other.Username, other.TotalCount.ToString(), other.WinCount.ToString());
        startButton.gameObject.SetActive(false);
    }

    public void SetTwoPlayersInfoAsync(UserData host, UserData other)
    {
        this.host = host;
        this.other = other;
    }

    public void SetHostPlayerInfo(string username, string totalCount, string winCount)
    {
        localPlayerUsername.text = username;
        localPlayerTotalCount.text = "总场数：" + totalCount;
        localPlayerWinCount.text = "胜场数：" + winCount;
    }

    public void SetOtherPlayerInfo(string username, string totalCount, string winCount)
    {
        enemyPlayerUsername.text = username;
        enemyPlayerTotalCount.text = "总场数：" + totalCount;
        enemyPlayerWinCount.text = "胜场数：" + winCount;
    }

    public void ClearOtherPlayer()
    {
        enemyPlayerUsername.text = "--等待加入--";
        enemyPlayerTotalCount.text = "-";
        enemyPlayerWinCount.text = "-";
    }

    public void ClearOtherPlayerAsync()
    {
        isOtherPlayerQuit = true;
    }

    void Update()
    {
        if (host != null && other == null)
        {
            SetHostPlayerInfo(host.Username, host.TotalCount.ToString(), host.WinCount.ToString());
            ClearOtherPlayer();
            host = null;
        }

        if(host != null && other != null)
        {
            SetHostPlayerInfo(host.Username, host.TotalCount.ToString(), host.WinCount.ToString());
            SetOtherPlayerInfo(other.Username, other.TotalCount.ToString(), other.WinCount.ToString());
            host = null;
            other = null;
        }

        if(host == null && other != null)
        {
            SetOtherPlayerInfo(other.Username, other.TotalCount.ToString(), other.WinCount.ToString());
            other = null;
        }

        if(isOtherPlayerQuit)
        {
            ClearOtherPlayer();
            isOtherPlayerQuit = false;
        }
    }

    private void EnterAnim()
    {
        gameObject.SetActive(true);

        localPlayerPanel.localPosition = new Vector3(-1000, 40);
        localPlayerPanel.DOLocalMoveX(-128, 0.5f);
        enemyPlayerPanel.localPosition = new Vector3(1000, 40);
        enemyPlayerPanel.DOLocalMoveX(128, 0.5f);
        startButton.localScale = Vector3.zero;
        startButton.DOScale(1, 0.5f);
        quitButton.localScale = Vector3.zero;
        quitButton.DOScale(1, 0.5f);
    }

    private void ExitAnim(TweenCallback action)
    {
        localPlayerPanel.DOLocalMoveX(-1000, 0.5f);
        enemyPlayerPanel.DOLocalMoveX(1000, 0.5f);
        startButton.DOScale(0, 0.5f);
        quitButton.DOScale(0, 0.5f).OnComplete(action);
    }

}
