using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePanel : BasePanel
{
    private Text timer;
    private int time = -1;
    private Button victoryBtn;
    private Button defeatBtn;
    private Button quitBtn;
    private AbortGameRequest abortGameRequest;

    void Awake()
    {
        timer = transform.Find("Timer").GetComponent<Text>();
        timer.gameObject.SetActive(false);       
    }

    void Start()
    {
        victoryBtn = transform.Find("Victory").GetComponent<Button>();
        defeatBtn = transform.Find("Defeat").GetComponent<Button>();
        quitBtn = transform.Find("Quit").GetComponent<Button>();
        victoryBtn.onClick.AddListener(OnResultButtonClick);
        defeatBtn.onClick.AddListener(OnResultButtonClick);
        quitBtn.onClick.AddListener(OnQuitButtonClick);
        victoryBtn.gameObject.SetActive(false);
        defeatBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        abortGameRequest = GetComponent<AbortGameRequest>();
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        victoryBtn.gameObject.SetActive(false);
        defeatBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowTime(int time)
    {
        timer.gameObject.SetActive(true);
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(()=>timer.gameObject.SetActive(false));

        GameFacade.Instance.PlayComSound(AudioManager.alertSound);
        if(time == 3)
        {
            quitBtn.gameObject.SetActive(true);
        }
    }

    public void ShowTimeAsync(int time)
    {
        this.time = time;
    }

    void Update()
    {
        if(time > -1)
        {
            ShowTime(time);
            time = -1;
        }
    }

    public void OnGameVectoryResponse()
    {
        GameFacade.Instance.DisableLocalPlayerControll();
        victoryBtn.gameObject.SetActive(true);
        victoryBtn.transform.localScale = Vector3.zero;
        victoryBtn.transform.DOScale(1, 0.5f);
    }

    public void OnGameDefeatResponse()
    {
        GameFacade.Instance.DisableLocalPlayerControll();
        defeatBtn.gameObject.SetActive(true);
        defeatBtn.transform.localScale = Vector3.zero;
        defeatBtn.transform.DOScale(1, 0.5f);
    }

    private void OnResultButtonClick()
    {
        GameFacade.Instance.GameOver();
        uiManager.PopPanel();
        uiManager.PopPanel();
        PlayClickSound();
    }

    private void OnQuitButtonClick()
    {
        abortGameRequest.SendRequest();      
    }

    public void OnAbortGameResponse()
    {
        OnResultButtonClick();
    }
}
