using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public PlayerController _playerController;
    private float healthVal; 
    private Image healthBar;
    
    private void Start()
    {
        healthBar = GetComponent<Image>();
        healthBar.color = Color.green;
    }

    private void Update()
    {
        healthVal = _playerController.playerHealth;
        SetHealthBar();
    }

    private void SetHealthBar()
    {
        if (healthVal >= 60f)
            healthBar.color = Color.green;
        else if (healthVal <= 60f && healthVal >= 20f)
            healthBar.color = Color.yellow;
        else
            healthBar.color = Color.red;
        
        healthBar.fillAmount = healthVal / 100f;
    }
}
