using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

/*
Important stuff:
    the number of animals and grass places are 8
*/

public class GameController : MonoBehaviour
{
    // Checking section
    public bool isNight = false;
    [SerializeField] private int maxNumAnimals = 8;
    private int animalsNum = 0;

    // Audio
    public AudioClip[] backGroundMusic;
    private AudioSource _audioSource;
    private int _chooseMusicRandom;

    // Particle System
    private ParticleSystem ps;

    // Animation
    public GameObject[] animals;

    // Other
    public Transform[] grassPlaces;



    //public GameObject cow;
    private Vector3 pos = new Vector3(0, 0, 0);


    private void Start()
    {
        // Music
        _audioSource = GetComponentInChildren<AudioSource>();
        PlayBackGrounMusic();

        // Instantiate(cow, spawnPos, Quaternion.identity);
        // StartCoroutine(Spawn());
    }

    // private IEnumerator Spawn()
    // {
    //     yield return new WaitForSeconds(2f);
    //     NavMeshHit hit;
    //     if(NavMesh.SamplePosition(spawnPos, out hit, 2f, 0))
    //     {
    //         Vector3 pos = hit.position;
    //         //cow.GetComponent<NavMeshAgent>().enabled = false;
    //         cow.GetComponent<NavMeshAgent>().Warp(pos);
    //         cow.transform.position = pos;
    //         //cow.GetComponent<NavMeshAgent>().enabled = true;
    //     }
    // }

    private void Update()
    {
        AnimalController();
    }

    private void PlayBackGrounMusic()
    {
        _chooseMusicRandom = Random.Range(0, backGroundMusic.Length - 1);
        _audioSource.clip = backGroundMusic[_chooseMusicRandom];
        _audioSource.Play();
    }

    private void AnimalController()
    {
        // the number of animals and grass places is 8
        // for(int i = 0; i < 8; i++)
        // {
        //     int animalRandomDestination = Random.Range(0, grassPlaces.Length - 1);
        //     Instantiate(animals[i], grassPlaces[i].position, Quaternion.identity);
        //     animals[i].GetComponent<NavMeshAgent>().SetDestination(grassPlaces[animalRandomDestination].position);
        //     if(animals[i].transform.position != grassPlaces[animalRandomDestination].position)
        //     {
        //         animals[i].GetComponent<Animator>().SetBool("changeDestination", true);
        //     }
        //     else if(animals[i].transform.position == grassPlaces[animalRandomDestination].position)
        //     {
        //         animals[i].GetComponent<Animator>().SetBool("changeDestination", false);
        //     }
        // }


        if(animalsNum <= maxNumAnimals)
        {
            int randAnimal = Random.Range(0, animals.Length - 1);
            int animalRandomInitialPos = Random.Range(0, grassPlaces.Length - 1);
            int animalRandomFinalPos = Random.Range(0, grassPlaces.Length - 1);
            GameObject chosenAnimal = animals[randAnimal];
            Vector3 spawnPosition = grassPlaces[animalRandomInitialPos].position;

            // Instantiating animals
            chosenAnimal = Instantiate(chosenAnimal, spawnPosition, Quaternion.identity);
            animalsNum++;

            // Setting the destination randomly between some grass places
            chosenAnimal.GetComponent<NavMeshAgent>().SetDestination(grassPlaces[animalRandomFinalPos].position);

            // Reached the destination
            if(chosenAnimal.transform.position != grassPlaces[animalRandomFinalPos].position)
            {
                chosenAnimal.GetComponent<Animator>().SetBool("changeDestination", true);
            }
            else if(chosenAnimal.transform.position == grassPlaces[animalRandomFinalPos].position)
            {
                chosenAnimal.GetComponent<Animator>().SetBool("changeDestination", false);
            }

        }
    }//AnimalController

    private void SetAnimalDestination()
    {

    }

}
