/*
 * 处理游戏开始前的初始化工作、游戏中的同步请求、游戏后的页面跳转和收尾工作
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PlayerManager : BaseManager {


    public UserData UserData { get; set; }
    public GameObject CurrentRoleGameObject
    {
        get
        {
            return currentRoleGameObject;
        }
    }

    private Dictionary<RoleType, RoleData> roleDataDict = new Dictionary<RoleType, RoleData>();
    private Transform birthplaces;

    private RoleType currentRoleType;
    private GameObject currentRoleGameObject;
    private GameObject playerSyncRequest;
    private GameObject remoteRoleGameObject;
    private SyncArrowRequest syncArrowRequest;
    private CauseDamageRequest causeDamageRequest;

    public PlayerManager(GameFacade facade) : base(facade) { }

    public override void OnInit()
    {
        birthplaces = GameObject.Find("Birthplaces").transform;
        InitRoleDataDict();
    }

    private void InitRoleDataDict()
    {
        roleDataDict.Add(RoleType.Blue, new RoleData(RoleType.Blue, "Hunter_BLUE", "BlueArrow", "BlueArrowExplosion", birthplaces.Find("Position1")));
        roleDataDict.Add(RoleType.Red, new RoleData(RoleType.Red, "Hunter_RED", "RedArrow", "RedArrowExplosion", birthplaces.Find("Position2")));
    }

    //获取服务器分配给当前客户端的角色游戏物体，以及另一个玩家角色游戏物体.
    public void InitRoles()
    {
        foreach(RoleData rd in roleDataDict.Values)
        {
            GameObject go = Object.Instantiate(rd.RolePrefab, rd.Birthplace, Quaternion.identity);
            go.tag = "Player";
            if(rd.RoleType == currentRoleType)
            {
                currentRoleGameObject = go;  
            }
            else
            {
                remoteRoleGameObject = go;
            }
        }
    }
   
    public void SetCurrentRoleType(RoleType rt)
    {
        currentRoleType = rt;
    }

    public void AddControlScripts()
    {
        RoleData currentRD = GetRoleData(currentRoleType);
        currentRoleGameObject.AddComponent<PlayerMove>();
        PlayerAttack playerAttack = currentRoleGameObject.AddComponent<PlayerAttack>();
        playerAttack.arrowPrefab = currentRD.ArrowPrefab;
        playerAttack.SetPlayerManager(this);
    }

    private RoleData GetRoleData(RoleType rt)
    {
        return roleDataDict.TryGet(rt);
    }

    public void InitGameSyncRequest()
    {
        playerSyncRequest = new GameObject("PlayerSyncRequest");
        playerSyncRequest.AddComponent<SyncMoveRequest>()
            .SetCurrentPlayer(currentRoleGameObject.transform, currentRoleGameObject.GetComponent<PlayerMove>())
            .SetRemotePlayer(remoteRoleGameObject.transform);

        syncArrowRequest = playerSyncRequest.AddComponent<SyncArrowRequest>();
        syncArrowRequest.PlayerManager = this;

        causeDamageRequest = playerSyncRequest.AddComponent<CauseDamageRequest>();
    }

    public void SendSyncArrowRequest(Vector3 position, Quaternion rotation)
    {
        syncArrowRequest.SendRequest(currentRoleType, position, rotation.eulerAngles);
    }
    //TODO:函数重命名
    public void Shoot(GameObject arrowPrefab, Vector3 position, Quaternion rotation)
    {
        GameObject.Instantiate(arrowPrefab, position, rotation).GetComponent<Arrow>().isLocal = true;
        GameFacade.Instance.PlayComSound(AudioManager.arrowShootSound);
        syncArrowRequest.SendRequest(currentRoleType, position, rotation.eulerAngles);
    }

    public void PlayRemotePlayerShootAnim()
    {
        remoteRoleGameObject.GetComponent<Animator>().SetTrigger("Attack");
    }

    public void RemotePlayerShoot(RoleType rt, Vector3 position, Vector3 rotataion)
    {
        GameObject arrowPrefab = GetRoleData(rt).ArrowPrefab;
        Transform arrowTransform = GameObject.Instantiate(arrowPrefab).GetComponent<Transform>();
        arrowTransform.position = position;
        arrowTransform.eulerAngles = rotataion;
    }

    public void SendCauseDamage(int damage)
    {
        causeDamageRequest.SendRequest(damage);
    }

    public void DisableLocalPlayerControll()
    {
        currentRoleGameObject.GetComponent<PlayerMove>().enabled = false;
        currentRoleGameObject.GetComponent<PlayerAttack>().enabled = false;
    }

    public void GameOver()
    {
        GameObject.Destroy(currentRoleGameObject);
        GameObject.Destroy(remoteRoleGameObject);
        GameObject.Destroy(playerSyncRequest);
        syncArrowRequest = null;
        causeDamageRequest = null;    
    }

    public void UpdateUserData(int totalCount, int winCount)
    {
        UserData.TotalCount = totalCount;
        UserData.WinCount = winCount;
    }
}
