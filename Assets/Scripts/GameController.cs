using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;


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


    private void Start()
    {
        // Music
        _audioSource = GetComponentInChildren<AudioSource>();
        PlayBackGrounMusic();

        // Animation
    }

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
        if(animalsNum <= maxNumAnimals)
        {
            int randAnimal = Random.Range(0, animals.Length - 1);
            int animalRandPlace = Random.Range(0, grassPlaces.Length - 1);
            GameObject chosenAnimal = animals[randAnimal];
            Vector3 spawnPosition = grassPlaces[animalRandPlace].position;

            // Instantiating animals
            chosenAnimal = Instantiate(chosenAnimal, spawnPosition, Quaternion.identity);
            animalsNum++;

            // Setting the destination randomly between some grass places 
            
            chosenAnimal.GetComponent<NavMeshAgent>().Warp(spawnPosition);
            chosenAnimal.GetComponent<NavMeshAgent>().SetDestination(spawnPosition);
            
            
            
            // Reached the destination
            if(chosenAnimal.transform.position != grassPlaces[animalRandPlace].position)
            {
                chosenAnimal.GetComponent<Animator>().SetBool("changeDestination", true);
            }
            else if(chosenAnimal.transform.position == grassPlaces[animalRandPlace].position)
            {
                chosenAnimal.GetComponent<Animator>().SetBool("changeDestination", false);
            }
       
        } 
    }
}
