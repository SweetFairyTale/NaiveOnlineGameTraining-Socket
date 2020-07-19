using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SyncArrowRequest : BaseRequest
{
    private PlayerManager playerManager;
    public PlayerManager PlayerManager
    {
        set
        {
            playerManager = value;
        }
    }

    private bool isShoot = false;
    private RoleType roleType;
    private Vector3 position;
    private Vector3 rotation;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.SyncArrow;
        base.Awake();
    }

    void FixedUpdate()
    {
        if(isShoot)
        {
            playerManager.PlayRemotePlayerShootAnim(); //先播放拉弓动画，延迟0.8s再实例化并飞出箭.
            Invoke("RemotePlayerShoot", 0.8f);
            isShoot = false;
        }
    }

    private void RemotePlayerShoot()
    {
        playerManager.RemotePlayerShoot(roleType, position, rotation);
    }

    public void SendRequest(RoleType roleType, Vector3 position, Vector3 rotation)
    {
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}",
            (int)roleType, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        roleType = (RoleType)int.Parse(strs[0]);
        position = GameTools.ParseVector3(strs[1]);
        rotation = GameTools.ParseVector3(strs[2]);
        isShoot = true;
    }
}
