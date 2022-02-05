using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TailSmasher : MonoBehaviour
{
    private AudioSource explosionSound;
    public GameObject impactParticle;

    private GameController gameController;
    [SerializeField] private float hitForce = 100f;

    private void Start()
    {
        explosionSound = GetComponent<AudioSource>();
    }
    
    private void OnCollisionEnter(Collision target)
    {
        switch(target.gameObject.tag)
        {
            case "Enemy":
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
                break;

            case "Animal":
                gameController.animalsNum--;
                target.gameObject.GetComponent<AnimalController>().isAlive = false;
                target.gameObject.GetComponent<Animator>().SetTrigger("dies");
                if(target.gameObject.GetComponent<AnimalController>().isAlive)
                    target.gameObject.GetComponent<GameController>().InstantiateAnimal(target.gameObject);
                break;



            default:
            break;
        }
 
    }

}
