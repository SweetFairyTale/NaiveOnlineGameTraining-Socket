using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{

    public Text username;
    public Text totalCount;
    public Text winCount;
    public Button joinButton;
    private int id;  //房间的ID，即房主的ID
    private RoomListPanel roomListPanel;

    void Start()
    {
        joinButton.onClick.AddListener(OnJoinButtonClick);
    }

    public void SetRoomItem(int id, string username, int totalCount, int winCount, RoomListPanel panel)
    {
        SetRoomItem(id, username, totalCount.ToString(), winCount.ToString(), panel);
    }

    public void SetRoomItem(int id, string username, string totalCount, string winCount, RoomListPanel panel)
    {
        this.id = id;
        this.username.text = username;
        this.totalCount.text = "总场数：" + totalCount;
        this.winCount.text = "胜场数：" + winCount;
        this.roomListPanel = panel;
    }

    private void OnJoinButtonClick()
    {
        GameFacade.Instance.PlayComSound(AudioManager.buttonClickSound);
        roomListPanel.OnJoinButtonClick(id);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
