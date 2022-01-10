using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private NavMeshAgent follower;
    private GameObject player;
    
    private void Start()
    {
        follower = GetComponent<NavMeshAgent>();
        // Accessing player's gameObject within the scene
        player = GameObject.FindWithTag("Player");
    }
    

    private void FixedUpdate()
    {
        follower.SetDestination(player.transform.position);
        //Vector3 playerDirection = transform.position - player.transform.position;
        //transform.Translate(playerDirection);
    }
    
}
