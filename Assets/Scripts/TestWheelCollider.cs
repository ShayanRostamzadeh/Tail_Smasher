using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWheelCollider : MonoBehaviour
{
    [Range(0, 8000)] public float motorForce;
    // [Range(-60, 60)] public float steerAngle;
    public WheelCollider[] frontWheels;
    public WheelCollider[] rearWheels;

    private float horizonralIn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(WheelCollider wheel in rearWheels)
        {
            wheel.motorTorque = motorForce;
        }
        horizonralIn = Input.GetAxis("Horizontal");
        foreach(WheelCollider wheel in frontWheels)
        {
            wheel.steerAngle = horizonralIn * 60f;
        }
        Debug.Log(horizonralIn * 60f);
    }

    private float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }
 
}
