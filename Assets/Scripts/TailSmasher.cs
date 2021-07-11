using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailSmasher : MonoBehaviour
{
    private AudioSource explosionSound;

    private void Start()
    {
        explosionSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.CompareTag("Enemy"))
        {
            explosionSound.Play();
            Destroy(target.gameObject);
        }
    }

    // private void OnCollisionEnter(Collision target)
    // {
    //     if (target.gameObject.CompareTag("Enemy"))
    //     {
    //         explosionSound.Play();
    //         Destroy(target.gameObject);
    //     }
    // }
}
