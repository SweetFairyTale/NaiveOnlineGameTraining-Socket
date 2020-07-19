using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyViaTime : MonoBehaviour
{
    public float time = 1;

    void Start()
    {
        Destroy(gameObject, time);
    }

}
