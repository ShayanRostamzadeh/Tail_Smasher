using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TailSmasher : MonoBehaviour
{
    private AudioSource explosionSound;
    public GameObject impactParticle;
    [SerializeField] private float hitForce = 100f;

    private void Start()
    {
        explosionSound = GetComponent<AudioSource>();
    }
    
    private void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.CompareTag("Enemy"))
        {
            // todo change the explosion sound
            explosionSound.Play();
            target.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            target.gameObject.GetComponent<EnemyFollow>().isAlive = false;
            
            //Vector3 offset = -(transform.position - target.transform.position);
            Vector3 offset = target.GetContact(0).point;
            Instantiate(impactParticle, offset, Quaternion.identity);
            impactParticle.GetComponent<ParticleSystem>().Play();
            target.rigidbody.AddForce(offset * hitForce, ForceMode.Impulse);
            Destroy(target.gameObject,4f);
        }
    }
    
    // private void OnTriggerEnter(Collider target)
    // {
    //     if (target.gameObject.CompareTag("Enemy"))
    //     {
    //         explosionSound.Play();
    //         Destroy(target.gameObject);
    //     }
    // }

}
