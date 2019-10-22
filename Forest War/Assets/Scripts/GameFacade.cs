using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour {

    private UIManager uiManager;
    private AudioManager audioManager;
    private CameraManager cameraManager;
    private PlayerManager playerManager;
    private RequestManager requestManager;
    private ClientManager clientManager;


	// Use this for initialization
	void Start () {
        InitManager();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitManager()
    {
        uiManager = new UIManager();
        audioManager = new AudioManager();
        cameraManager = new CameraManager();
        playerManager = new PlayerManager();
        requestManager = new RequestManager();
        clientManager = new ClientManager();

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
}
