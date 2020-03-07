using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

//单例
public class GameFacade : MonoBehaviour {

    private static GameFacade _instance;
    public static GameFacade Instance
    {
        get
        {
            return _instance;
        }
    }

    private UIManager uiManager;
    private AudioManager audioManager;
    private CameraManager cameraManager;
    private PlayerManager playerManager;
    private RequestManager requestManager;
    private ClientManager clientManager;

    
    void Awake()
    {
        if(_instance != null)  //避免场景中有多个实例.
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

	// Use this for initialization
	void Start () {
        InitManager();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateManager();
	}

    private void OnDestroy()
    {
        DestoryManager();
    }

    private void InitManager()
    {
        uiManager = new UIManager(this);
        audioManager = new AudioManager(this);
        cameraManager = new CameraManager(this);
        playerManager = new PlayerManager(this);
        requestManager = new RequestManager(this);
        clientManager = new ClientManager(this);

        uiManager.OnInit();
        audioManager.OnInit();
        cameraManager.OnInit();
        playerManager.OnInit();
        requestManager.OnInit();
        clientManager.OnInit();
    }

    private void DestoryManager()
    {
        uiManager.OnDestroy();
        audioManager.OnDestroy();
        cameraManager.OnDestroy();
        playerManager.OnDestroy();
        requestManager.OnDestroy();
        clientManager.OnDestroy();
    }

    //对需要处理异步回调但又没有继承自MonoBehaviour的类，让GameFacade提供Update监听.
    private void UpdateManager()  
    {
        uiManager.Update();
    }

    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        requestManager.AddRequest(actionCode, request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestManager.RemoveRequest(actionCode);
    }

    /// <summary>
    /// 转发ClientManager的处理请求到RequestManager，进而调用对应BaseRequest的派生类处理.
    /// △注意：Response的所有处理处于异步线程中，无法直接访问Unity资源.
    /// </summary>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestManager.HandleResponse(actionCode, data);
    }
       
    /// <summary>
    /// 使用Message面板显示提示信息.
    /// </summary>
    /// <param name="data"></param>
    public void ShowMessage(string data)
    {
        uiManager.ShowMessage(data);
    }

    /// <summary>
    /// 向服务器端发送请求.
    /// </summary>
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        clientManager.SendRequest(requestCode, actionCode, data);
    }

    public void PlayBgSound(string soundName)
    {
        audioManager.PlayBgSound(soundName);
    }

    public void PlayComSound(string soundName)
    {
        audioManager.PlayComSound(soundName);
    }

    public void SetUserData(UserData ud)
    {
        playerManager.UserData = ud;
    }

    public UserData GetUserData()
    {
        return playerManager.UserData;
    }
}
