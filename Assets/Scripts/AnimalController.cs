using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalController : MonoBehaviour
{
    public bool isAlive;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    void Start()
    {
        isAlive = true;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        InvokeRepeating("ChangeAnimalDestination", 1f, 4f);
    }

    void Update()
    {
        if(isAlive)
        {
            PlayAnimation();
        }
        else if(!isAlive)
        {
            Destroy(this.gameObject, 3f);
        }
    }

    public void PlayAnimation()
    {
        if(!navMeshAgent.isStopped)
        {
            animator.SetBool("changeDestination", true);
        }
        // todo check why the following animation won't play???????????????????
        else if(navMeshAgent.remainingDistance <= 2f)
        {
            animator.SetBool("changeDestination", false);
        }
    }


    private void ChangeAnimalDestination()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-110f, 110f), 0, Random.Range(-110f, 110f));

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas))
        {
            randomPosition = hit.position;
            navMeshAgent.SetDestination(randomPosition);
        }
    }




}
