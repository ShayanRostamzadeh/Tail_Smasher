using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiButtonPlayerProber : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool turnRight = false;
    public bool turnLeft = false;
    
    private GameObject playerReference;
    private PlayerController _playerScript;

    private void Start()
    {
        playerReference = GameObject.FindWithTag("Player");
        _playerScript = playerReference.GetComponent<PlayerController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (turnLeft)
            _playerScript.TurnLeft();
        else if(turnRight)
            _playerScript.TurnRight();
        
            //TODO check the functionality && functions only ________WHEN COLLIDED WITH WALLS________
        else if (turnLeft && turnRight)
            _playerScript.Reverse();
        else
        {
            _playerScript.forwardForceEnable = true;  
            foreach (var light in _playerScript.rearLights)
            {
                if (light.activeSelf)
                    light.SetActive(false);
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData) => _playerScript.PointerUp();
}
