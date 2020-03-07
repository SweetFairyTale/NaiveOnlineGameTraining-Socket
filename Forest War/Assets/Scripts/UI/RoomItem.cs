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


    void Start()
    {
         if(joinButton != null)
        {
            joinButton.onClick.AddListener(OnJoinButtonClick);
        }
    }

    public void SetRoomItem(string username, int totalCount, int winCount)
    {
        this.username.text = username;
        this.totalCount.text = "总场数：" + totalCount;
        this.winCount.text = "胜场数：" + winCount;
    }

    public void SetRoomItem(string username, string totalCount, string winCount)
    {
        this.username.text = username;
        this.totalCount.text = "总场数：" + totalCount;
        this.winCount.text = "胜场数：" + winCount;
    }

    private void OnJoinButtonClick()
    {

    }
}
