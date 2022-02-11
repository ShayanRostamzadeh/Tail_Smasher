using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalController : MonoBehaviour
{
    public bool isAlive;
    public bool isMoving = true;
    public float remainingDistanceThreshold = 3f;
    private float defaultStateLength;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private AudioSource audioSource;
    private GameController _gameController;


    void Start()
    {
        isAlive = true;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        _gameController = FindObjectOfType<GameController>();

        // Randomly playing audioclips
        InvokeRepeating("AudioPlay", Random.Range(1f, 10f), Random.Range(20f, 30f));

        defaultStateLength = animator.GetCurrentAnimatorStateInfo(0).length;

    }//Start

    void Update()
    {
        //Debug.Log("Remaining distance: " + navMeshAgent.remainingDistance);
        if(isAlive)
        {
            PlayAnimation();
        }
        else if(!isAlive)
        {
            _gameController.InstantiateAnimal(tag);
            //Debug.Log(tag);
            Destroy(this.gameObject, 2.5f);
        }

        
    }//Update

    public void PlayAnimation()
    {
        isMoving = true;

        // not yet reached the destination
        if(navMeshAgent.remainingDistance > remainingDistanceThreshold && isMoving)
        {
            animator.SetBool("changeDestination", true);
        }
        // reached the destination
        else if(navMeshAgent.remainingDistance <= remainingDistanceThreshold)
        {
            animator.SetBool("changeDestination", false);
            Invoke("ChangeAnimalDestination", defaultStateLength * 2f);
            isMoving = false;
        }
    
    }//PlayAnimation


    private void ChangeAnimalDestination()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-110f, 110f), 0, Random.Range(-110f, 110f));

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPosition, out hit, 1f, NavMesh.AllAreas) && !isMoving)
        {
            randomPosition = hit.position;
            navMeshAgent.SetDestination(randomPosition);
        }
    }//ChangeAnimalDestination

    private void AudioPlay()
    {
        audioSource.Play();
    }
}
