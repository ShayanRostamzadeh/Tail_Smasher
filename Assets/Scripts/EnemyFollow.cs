using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    // this scripts references the enemy game objects using String
    // todo change if possible
    public String enemyName; 
    private NavMeshAgent hitter;
    private GameObject player;
    public bool isAlive = true;
    public float enemySpeed;
    
    private void Start()
    {
        switch (enemyName)
        {
            case "hitter":
                hitter = GetComponent<NavMeshAgent>();
                break;
            case "shooter":
                // todo complete this section
                break;
            case "exploder":
                // todo complete this section
                break;
            default:
                break;
        }
        
        // Player's reference in scene
        player = GameObject.FindWithTag("Player");
    }


    private void FixedUpdate()
    {
        if (isAlive)
        {
            // Hitter
            hitter.speed = enemySpeed;
            //hitter.gameObject.GetComponent<Rigidbody>().MoveRotation();
            hitter.SetDestination(player.transform.position);
            
            // Shooter
            
            
            // Exploder
        }
    }

}
