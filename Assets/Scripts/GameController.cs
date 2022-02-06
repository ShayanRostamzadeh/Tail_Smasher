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
    [HideInInspector] public int animalsNum = 0;

    // Audio
    public AudioClip[] backGroundMusic;
    private AudioSource _audioSource;
    private int _chooseMusicRandom;

    // Particle System
    private ParticleSystem ps;

    // Animation
    public GameObject[] animals;

    // Other




    //public GameObject cow;
    private Vector3 pos = new Vector3(0, 0, 0);


    private void Start()
    {
        // Music
        _audioSource = GetComponentInChildren<AudioSource>();
        PlayBackGrounMusic();

        AnimalInitialInstantiation();

    }


    private void FixedUpdate()
    {
        
    }

    private void PlayBackGrounMusic()
    {
        _chooseMusicRandom = Random.Range(0, backGroundMusic.Length - 1);
        _audioSource.clip = backGroundMusic[_chooseMusicRandom];
        _audioSource.Play();
    }


    #region Animal

    private void AnimalInitialInstantiation()
    {
        // Instantiation and destination set
        foreach(GameObject animal in animals)
        {
            if(animalsNum < maxNumAnimals)
            {
            Instantiate(animal, SetAnimalSpawnPos(), Quaternion.identity);
            animalsNum++;
            }
        }
    }//AnimalInitialInstantiation

    public void InstantiateAnimal(String tag)
    {
        if(animalsNum <= maxNumAnimals)
        {
            for(int i = 0; i < animals.Length; i++)
            {
                if(animals[i].CompareTag(tag))
                {
                    Debug.Log("Instantiating animal...........");
                    Instantiate(animals[i], SetAnimalSpawnPos(), Quaternion.identity);
                    animalsNum++;
                    continue;
                }
            }
        }

    }

    private Vector3 SetAnimalSpawnPos()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-110f, 110f), 0, Random.Range(-110f, 110f));

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas))
        {
            randomPosition = hit.position;
        }
        return randomPosition;
    }//SetAnimalSpawnPos

    #endregion Animal

}
