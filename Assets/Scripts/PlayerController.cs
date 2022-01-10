using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerController : MonoBehaviour
{
    public float forwardForce = 3000f;
    public float rotationFactor = 50f;
    public bool forwardForceEnable = true;
    private float carTiltDelay = 0f;
    private bool turnRight = false;
    private bool turnLeft = false;

    [SerializeField] private GameObject carBody;
    [SerializeField] private Transform centerOfMass;
    public GameObject[] frontLights;
    public GameObject[] rearLights;

    public Animator animator;
    private Rigidbody playerRigidbody;
    private ParticleSystem explosionEffect;
    private AudioSource explosionSound;
    public GameController _gameController;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        explosionEffect = GetComponent<ParticleSystem>();
        explosionSound = GetComponent<AudioSource>();
        
        // Animation section
        //animator = gameObject.GetComponentInChildren<Animator>();
        //if(animator.transform.gameObject.name == "Player_Car_01")

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

        // The player continuously goes forward without the control of user 
        if(forwardForceEnable)
            ForceForward();
        // Checking if UI buttons are pressed to steer accordingly 
        Steer();
        
        // todo remove this section
        #region ReverseGear
        while (Input.GetKeyDown(KeyCode.Space))
            Reverse();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            forwardForceEnable = true;
            foreach (var light in rearLights)
            {
                if (light.activeSelf)
                    light.SetActive(false);
            }
        }
        #endregion
    }

    public void TurnRight() => turnRight = true;
    public void TurnLeft() => turnLeft = true;
    
    public void Reverse()
    {
        Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
        playerRigidbody.velocity = -movement;
        forwardForceEnable = false;
        foreach (var light in rearLights)
        {
            if (!light.activeSelf)
                light.SetActive(true);
        }
    }//Reverse()

    public void PointerUp()
    {
        turnRight = false;
        turnLeft = false;
    }
    
    #region Explosion
    
    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.CompareTag("Enemy"))
        {
            explosionEffect.Play();
            explosionSound.Play();
            DeathSequence();
        }
    }

    // private void OnCollisionEnter(Collision target)
    // {
    //     if (target.gameObject.CompareTag("Enemy"))
    //     {
    //         explosionEffect.Play();
    //         explosionSound.Play();
    //         DeathSequence();
    //     }
    // }

    private void DeathSequence()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        Destroy(transform.Find("Chains").gameObject);
        forwardForceEnable = false;
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 offset = (rb.position + playerRigidbody.position) / 2;
                rb.AddExplosionForce(9000f, offset, 50f, 1000f, ForceMode.Impulse);
                Destroy(gameObject, 0.5f);
            }
        }
    }
    #endregion
    
    #region Movement

    private void ForceForward()
    {
        Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
        playerRigidbody.velocity = movement;
    } //ForceForward()

    private void Steer()
    {
        if (turnLeft)
        {
            animator.SetBool("turnLeft", true);
            // carBody.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, -Vector3.forward * 8, carTiltDelay));
            playerRigidbody.MoveRotation(Quaternion.Euler(-Vector3.up * (Time.deltaTime * rotationFactor)) * transform.rotation);
            // carTiltDelay += 0.1f;
        }
        else if (turnRight)
        {
            animator.SetBool("turnRight", true);
            // carBody.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, Vector3.forward * 8, carTiltDelay));
            playerRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * (Time.deltaTime * rotationFactor)) *
                                         transform.rotation);
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
