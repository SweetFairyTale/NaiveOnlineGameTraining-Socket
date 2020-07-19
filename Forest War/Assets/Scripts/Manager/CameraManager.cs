using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : BaseManager
{
    private GameObject cameraGo;
    private Animator cameraAnim;
    private PlayerCamera playerCamera;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    public CameraManager(GameFacade facade) : base(facade) { }

    public override void OnInit()
    {
        cameraGo = Camera.main.gameObject;
        cameraAnim = cameraGo.GetComponent<Animator>();
        playerCamera = cameraGo.GetComponent<PlayerCamera>();
        originalPosition = cameraGo.transform.position;
        originalRotation = cameraGo.transform.eulerAngles;
    }

    ////测试相机切换代码，由GameFacade间接实现的Update.
    //public override void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        EnablePlayerFollowing((GameObject.Find("Hunter_BLUE") as GameObject).transform);
    //    }

    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        EnableRoamCamera();
    //    }
    //}

    public void EnablePlayerFollowing()
    {
        Transform target = GameFacade.Instance.GetCurrentRoleGameObject().transform;
        cameraAnim.enabled = false;

        //记录激活相机跟随时漫游到的位置.
        originalPosition = cameraGo.transform.position;
        originalRotation = cameraGo.transform.eulerAngles;

        Quaternion targetQuaternion = Quaternion.LookRotation(target.position - cameraGo.transform.position);
        cameraGo.transform.DORotateQuaternion(targetQuaternion, 1f).OnComplete(() =>
        {
            playerCamera.enabled = true;
            playerCamera.target = target;
        });
    }

    public void EnableRoamCamera()
    {
        playerCamera.enabled = false;  //必须先禁用playerCamera脚本，否则会影响相机漫游运动.
        cameraGo.transform.DOMove(originalPosition, 1f);
        cameraGo.transform.DORotate(originalRotation, 1f).OnComplete(() => cameraAnim.enabled = true);
    }

}
