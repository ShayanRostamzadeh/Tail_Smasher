using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardForce = 30f;
    public float rotationFactor = 50f;
    private bool forwardForceEnable = true;
    private float carTiltDelay = 0f;
    private bool turnRight = false;
    private bool turnLeft = false;

    [SerializeField] private GameObject carBody;
    [SerializeField] private Transform centerOfMass;

    private Rigidbody playerRigidbody;
    private ParticleSystem explosionEffect;
    private AudioSource explosionSound;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        explosionEffect = GetComponent<ParticleSystem>();
        explosionSound = GetComponent<AudioSource>();
    }
    
    private void FixedUpdate()
    {
        playerRigidbody.centerOfMass = centerOfMass.localPosition;

        // The player continuously goes forward without the control of user 
        if(forwardForceEnable)
            ForceForward();
        // Checking if UI buttons are pressed to steer accordingly 
        Steer();
    }

    public void TurnRight() => turnRight = true;
    public void TurnLeft() => turnLeft = true;

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
                Destroy(gameObject, 2f);
            }
        }
    }
    #endregion
    
    #region Movement

    private void ForceForward()
    {
        Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
        playerRigidbody.MovePosition(playerRigidbody.position + movement);
    } //ForceForward()

    private void Steer()
    {
        if (turnLeft)
        {
            carBody.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, -Vector3.forward * 8, carTiltDelay));
            playerRigidbody.MoveRotation(Quaternion.Euler(-Vector3.up * (Time.deltaTime * rotationFactor)) * transform.rotation);
            carTiltDelay += 0.1f;
        }
        else if (turnRight)
        {
            carBody.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, Vector3.forward * 8, carTiltDelay));
            playerRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * (Time.deltaTime * rotationFactor)) *
                                         transform.rotation);
            carTiltDelay += 0.1f;
        }
        else
        {
            carBody.transform.localRotation = Quaternion.identity;
            carTiltDelay = 0f;
        }
    } //Steer()

    #endregion

}
