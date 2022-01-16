using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float forwardForce = 3000f;
    public float rotationFactor = 50f;
    [HideInInspector] public float playerHealth = 100f;
    
    public bool forwardForceEnable = true;
    private bool turnRight = false;
    private bool turnLeft = false;
    //private float carTiltDelay = 0f;
    
    [SerializeField] private Transform centerOfMass;
    [HideInInspector] public GameObject[] frontLights;
    [HideInInspector] public GameObject[] rearLights;
    public Animator animator;
    private Rigidbody playerRigidbody;
    public GameObject carBodyFractured;
    public GameController _gameController;
    
    private AudioSource audioSource;
    public AudioClip carDriftSoundClip;

    private void Start()
    {
        // Physics
        playerRigidbody = GetComponent<Rigidbody>();
        
        // Audio 
        audioSource = GetComponent<AudioSource>();


        // todo: how to append one array to another????????
        if (!_gameController.isNight)
        {
            frontLights = GameObject.FindGameObjectsWithTag("FrontLight");
            rearLights = GameObject.FindGameObjectsWithTag("RearLight");
            foreach (GameObject light in frontLights)
                light.SetActive(false);
            foreach (GameObject light in rearLights)
                light.SetActive(false);
        }
    }
    
    private void FixedUpdate()
    {
        playerRigidbody.centerOfMass = centerOfMass.localPosition;

        #region Movement
        
        // The player continuously goes forward without the control of user 
        if(forwardForceEnable)
            ForceForward();
        
        // Checking if UI buttons are pressed to steer accordingly 
        Steer();
        Reverse();
        
        #endregion
    }
    

    #region Button Controller
    public void TurnRight() => turnRight = true;
    public void TurnLeft() => turnLeft = true;
    
    public void ButtonPointerUp()
    {
        turnRight = false;
        turnLeft = false;
    }//ButtonPointerUp
    #endregion
    
    
    #region Impact
    
    // private void OnTriggerEnter(Collider target)
    // {
    //     if (target.gameObject.CompareTag("Enemy"))
    //     {
    //         DeathSequence();
    //     }
    // }

    private void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.CompareTag("Enemy"))
        {
            GetDamage(target.gameObject);
            if (playerHealth <= 0f) 
            {
                DeathSequence();
            }
        }
    }
    
    private void GetDamage(GameObject enemy)
    {
        string name = enemy.gameObject.GetComponent<EnemyFollow>().enemyName;
        Debug.Log(name);
        switch (name)
        {
            case "hitter":
                playerHealth -= 30f;
                break;
            case "Shooter":
                playerHealth -= 15f;
                break;
            case "exploder":
                playerHealth -= 70f;
                break;
            default:
                break;
        }
    }

    private void DeathSequence()
    {
        forwardForceEnable = false;
        Instantiate(carBodyFractured, transform.position, Quaternion.identity);
        carBodyFractured.GetComponent<Rigidbody>().AddExplosionForce(10000f, transform.position, 100f);
        Destroy(gameObject);
    }
    #endregion
    
    #region Movement
    private void ForceForward()
    {
        Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
        playerRigidbody.velocity = movement;
    } //ForceForward()
    
    public void Reverse()
    {
        if (turnLeft && turnRight)
        {
            forwardForceEnable = false;
            Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
            playerRigidbody.velocity = -movement;
            foreach (var light in rearLights)
            {
                if (!light.activeSelf)
                    light.SetActive(true);
            }
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            forwardForceEnable = false;
            Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
            playerRigidbody.velocity = -movement;
            foreach (var light in rearLights)
            {
                if (!light.activeSelf)
                    light.SetActive(true);
            }
        }
        else
        {
            forwardForceEnable = true;
            foreach (var light in rearLights)
            {
                if (light.activeSelf)
                    light.SetActive(false);
            }
        }

    }//Reverse()

    private void Steer()
    {
        if (turnLeft && forwardForceEnable)
        {
            animator.SetBool("turnLeft", true);
            playerRigidbody.MoveRotation(Quaternion.Euler(-Vector3.up * (Time.deltaTime * rotationFactor)) * transform.rotation);
            audioSource.PlayOneShot(carDriftSoundClip);
            // carBody.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, -Vector3.forward * 8, carTiltDelay));
            // carTiltDelay += 0.1f;
        }
        else if (turnRight && forwardForceEnable)
        {
            animator.SetBool("turnRight", true);
            playerRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * (Time.deltaTime * rotationFactor)) * transform.rotation);
            audioSource.PlayOneShot(carDriftSoundClip);
            // carBody.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, Vector3.forward * 8, carTiltDelay));
            // carTiltDelay += 0.1f;
        }
        else
        {
            animator.SetBool("turnLeft", false);
            animator.SetBool("turnRight", false);
            //carBody.transform.localRotation = Quaternion.identity;
            //carTiltDelay -= 0.1f;
        }
    } //Steer()

    #endregion

}
