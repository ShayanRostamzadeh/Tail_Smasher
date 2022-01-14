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
    public bool forwardForceEnable = true;
    //private float carTiltDelay = 0f;
    private bool turnRight = false;
    private bool turnLeft = false;
    
    [SerializeField] private Transform centerOfMass;
    [HideInInspector] public GameObject[] frontLights;
    [HideInInspector] public GameObject[] rearLights;
    
    public Animator animator;
    private Rigidbody playerRigidbody;
    public GameObject carBodyFractured;
    public GameController _gameController;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

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
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        forwardForceEnable = false;

        // todo change to destructible object
        Instantiate(carBodyFractured, transform.position, Quaternion.identity);
        carBodyFractured.GetComponent<Rigidbody>().AddExplosionForce(10000f, transform.position, 100f);
        Destroy(gameObject);
        
        //Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        // foreach (Collider hit in colliders)
        // {
        //     Rigidbody rb = hit.GetComponent<Rigidbody>();
        //     if (rb != null)
        //     {
        //         Vector3 offset = (rb.position + playerRigidbody.position) / 2;
        //         rb.AddExplosionForce(100000f, offset, 100f);
        //         Destroy(gameObject, 1f);
        //     }
        // }
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
            Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
            playerRigidbody.velocity = -movement;
            forwardForceEnable = false;
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
        if (turnLeft)
        {
            animator.SetBool("turnLeft", true);
            playerRigidbody.MoveRotation(Quaternion.Euler(-Vector3.up * (Time.deltaTime * rotationFactor)) * transform.rotation);
            
            // carBody.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, -Vector3.forward * 8, carTiltDelay));
            // carTiltDelay += 0.1f;
        }
        else if (turnRight)
        {
            animator.SetBool("turnRight", true);
            playerRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * (Time.deltaTime * rotationFactor)) * transform.rotation);
            
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
