using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

// This script has references to enemy within the scene

public class Timer : MonoBehaviour
{
    private Text showTime;
    public float time ;
    private string timeString;

    private GameObject[] enemies;
    private int enemyNum;
    private float timer = 0f;

    private void Start()
    {
        showTime = GetComponent<Text>();
    }

    private void Update()
    {
        timer = time - Time.timeSinceLevelLoad;
        timeString = timer.ToString("f2");
        showTime.text = "Remaining : " + timeString;
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyNum = enemies.Length;
        if (enemyNum == 0)
        {
            showTime.color = Color.magenta;
            showTime.text = "Victorious";
            StartCoroutine(SceneReload("enterance"));
        }

        if (!GameObject.FindWithTag("Player"))
            StartCoroutine(SceneReload("entrance"));
            
        if (timer <= 0f)
        {
            if (enemyNum != 0)
            {
                showTime.text = "You Lose";
                showTime.color = Color.red;
            }
            StartCoroutine(SceneReload("level1"));
        }
    }

    private IEnumerator SceneReload(string name)
    {
        yield return new WaitForSeconds(2.5f);
        if (name == "level1")
            SceneManager.LoadScene("Level1");
        else if (name == "enterance")
            SceneManager.LoadScene("Enterance");
    }
}
