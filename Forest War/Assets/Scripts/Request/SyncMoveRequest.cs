using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SyncMoveRequest : BaseRequest
{
    //本地当前角色
    private Transform currentPlayerTransform;
    private PlayerMove currrentPlayerMove;
    
    //本地创建的远程角色
    private Transform remotePlayerTransform;
    private Animator remotePlayerAnim;

    //同步远程玩家移动参数
    private bool isSyncRemotePlayer = false;
    private Vector3 position;
    private Vector3 rotation;
    private float animForward;

    private int syncRate = 30;  //times per second.

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.SyncMove;
        base.Awake();
    }

    void Start()
    {
        InvokeRepeating("SyncCurrentPlayerMove", 0.5f, 1f / syncRate);
    }

    void FixedUpdate()
    {
        if(isSyncRemotePlayer)
        {
            SyncRemotePlayerMove();
            isSyncRemotePlayer = false;
        }
    }

    public SyncMoveRequest SetCurrentPlayer(Transform currentPlayerTransform, PlayerMove currrentPlayerMove)
    {
        this.currentPlayerTransform = currentPlayerTransform;
        this.currrentPlayerMove = currrentPlayerMove;
        return this;
    }

    public SyncMoveRequest SetRemotePlayer(Transform remotePlayerTransform)
    {
        this.remotePlayerTransform = remotePlayerTransform;
        remotePlayerAnim = remotePlayerTransform.GetComponent<Animator>();
        return this;
    }

    //发起同步本地角色信息的请求.
    private void SyncCurrentPlayerMove()
    {
        SendRequest(currentPlayerTransform.position.x, currentPlayerTransform.position.y, currentPlayerTransform.position.z,
            currentPlayerTransform.eulerAngles.x, currentPlayerTransform.eulerAngles.y, currentPlayerTransform.eulerAngles.z,
            currrentPlayerMove.animForward);
    }

    public void SyncRemotePlayerMove()
    {
        remotePlayerTransform.position = position;
        remotePlayerTransform.eulerAngles = rotation;
        remotePlayerAnim.SetFloat("Forward", animForward);
    }

    private void SendRequest(float x, float y, float z, float rotationX, float rotationY, float rotationZ, float animForward)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", x, y, z, rotationX, rotationY, rotationZ, animForward);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        position = GameTools.ParseVector3(strs[0]);
        rotation = GameTools.ParseVector3(strs[1]);
        animForward = float.Parse(strs[2]);
        isSyncRemotePlayer = true;
    }
}
