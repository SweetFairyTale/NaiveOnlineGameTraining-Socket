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

    private RectTransform bluePanel;
    private RectTransform redPanel;
    private RectTransform startButton;
    private RectTransform quitButton;

    void Start()
    {
        localPlayerUsername = transform.Find("BluePanel/Username").GetComponent<Text>();
        localPlayerTotalCount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        localPlayerWinCount = transform.Find("BluePanel/WinCount").GetComponent<Text>();

        enemyPlayerUsername = transform.Find("RedPanel/Username").GetComponent<Text>();
        enemyPlayerTotalCount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        enemyPlayerWinCount = transform.Find("RedPanel/WinCount").GetComponent<Text>();

        bluePanel = transform.Find("BluePanel").GetComponent<RectTransform>();
        redPanel = transform.Find("RedPanel").GetComponent<RectTransform>();
        startButton = transform.Find("StartButton").GetComponent<RectTransform>();
        quitButton = transform.Find("QuitButton").GetComponent<RectTransform>();

        transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartButtonClick);
        transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(OnQuitButtonClick);
    }

    public override void OnEnter()
    {
        
    }

    public void OnStartButtonClick()
    {

    }

    public void OnQuitButtonClick()
    {

    }

    private void SetLocalPlayerInfo(string username, string totalCount, string winCount)
    {
        localPlayerUsername.text = username;
        localPlayerTotalCount.text = "总场数：" + totalCount;
        localPlayerWinCount.text = "胜场数：" + winCount;
    }

    private void SetEnemyPlayerInfo(string username, string totalCount, string winCount)
    {
        enemyPlayerUsername.text = username;
        enemyPlayerTotalCount.text = "总场数：" + totalCount;
        enemyPlayerWinCount.text = "胜场数：" + winCount;
    }

    //清空对战玩家消息面板.
    private void ClearEnemyPlayer()
    {
        enemyPlayerUsername.text = "";
        enemyPlayerTotalCount.text = "";
        enemyPlayerWinCount.text = "";
    }

    private void EnterAnim()
    {
        bluePanel.localPosition = new Vector3(-1000, 0);
        bluePanel.DOMoveX(-128, 0.5f);
        redPanel.localPosition = new Vector3(1000, 0);
        redPanel.DOMoveX(128, 0.5f);
    }

    private void ExitAnim()
    {

    }

}
