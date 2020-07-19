using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    public GameObject arrowPrefab;  //在PlayerManager中赋值.
    private Transform arrowInitPos;
    private PlayerMove playerMoveController;
    private PlayerManager playerManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        arrowInitPos = transform.
            Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/Bip001 R Hand Prop");
        playerMoveController = GetComponent<PlayerMove>();
    }

    void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollide = Physics.Raycast(ray, out hit);
                if(isCollide)
                {
                    Shoot(hit.point);
                }
            }
        }
    }

    Vector3 dir;
    private void Shoot(Vector3 targetPoint)
    {
        playerMoveController.DisableMoveControll = true;
        anim.SetTrigger("Attack");  //先播放拉弓动画，延迟0.8s再实例化并飞出箭.

        targetPoint.y = transform.position.y;
        dir = targetPoint - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);

        playerManager.SendSyncArrowRequest(arrowInitPos.position, Quaternion.LookRotation(dir));
        Invoke("InstantiateArrow", 0.8f);
    }

    private void InstantiateArrow()
    {
        Instantiate(arrowPrefab, arrowInitPos.position, Quaternion.LookRotation(dir)).GetComponent<Arrow>().isLocal = true;
        playerMoveController.DisableMoveControll = false;
    }

    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }
}
