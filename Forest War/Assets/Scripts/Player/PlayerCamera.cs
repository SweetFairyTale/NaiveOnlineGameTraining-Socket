using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [HideInInspector]
    public Transform target;

    private Vector3 offset = new Vector3(0f, 11f, -10f);
    private float smooth = 2f;

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);
        transform.LookAt(target);
    }
}
