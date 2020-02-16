using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

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

    private void OnDestroy()
    {
        DestoryManager();
    }

    public void AddRequest(RequestCode requestCode, BaseRequest request)
    {
        requestManager.AddRequest(requestCode, request);
    }

    public void RemoveRequest(RequestCode requestCode)
    {
        requestManager.RemoveRequest(requestCode);
    }

    /// <summary>
    /// 转发ClientManager的处理请求到RequestManager，进而调用对应的BaseRequest处理
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    public void HandleResponse(RequestCode requestCode, string data)
    {
        requestManager.HandleResponse(requestCode, data);
    }
}
