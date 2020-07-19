using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool isLocal = false;
    private float speed = 25;
    private Rigidbody rgd;
    public GameObject explosionEffect;

    void Start()
    {
        rgd = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rgd.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameFacade.Instance.PlayComSound(AudioManager.shootPersonSound);
            if(isLocal)
            {
                GameFacade.Instance.SendCauseDamage(Random.Range(8, 10));
            }
        }
        else
        {
            GameFacade.Instance.PlayComSound(AudioManager.missSound);
        }

        GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
