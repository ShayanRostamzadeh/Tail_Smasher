using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //TODO use cinamachion

    private GameObject player;
    private Vector3 offsetFromPlayer;
    public float smoothFactor = 0.3f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offsetFromPlayer = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position + offsetFromPlayer;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFactor * Time.deltaTime);
        transform.position = smoothedPosition;
        if(player.activeSelf)
            transform.LookAt(player.transform);
    }
}
