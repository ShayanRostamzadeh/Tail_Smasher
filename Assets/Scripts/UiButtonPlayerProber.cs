using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiButtonPlayerProber : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool turnRight = false;
    [SerializeField] private bool turnLeft = false;
    
    private GameObject playerReference;
    private PlayerController playerScript;

    private void Start()
    {
        playerReference = GameObject.FindWithTag("Player");
        playerScript = playerReference.GetComponent<PlayerController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (turnLeft)
            playerScript.TurnLeft();
        else if(turnRight)
            playerScript.TurnRight();
    }
    public void OnPointerUp(PointerEventData eventData) => playerScript.PointerUp();
}
