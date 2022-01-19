using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    public AudioClip[] backGroundMusic;
    private AudioSource _audioSource;
    public bool isNight = false;
    private int _chooseMusicRandom;
    private ParticleSystem ps;

    private void Start()
    {
        // Music
        _audioSource = GetComponentInChildren<AudioSource>();
        PlayBackGrounMusic();
    }

    private void PlayBackGrounMusic()
    {
        _chooseMusicRandom = Random.Range(0, backGroundMusic.Length - 1);
        _audioSource.clip = backGroundMusic[_chooseMusicRandom];
        _audioSource.Play();
        Debug.Log("Shayannnnn");
    }
}
