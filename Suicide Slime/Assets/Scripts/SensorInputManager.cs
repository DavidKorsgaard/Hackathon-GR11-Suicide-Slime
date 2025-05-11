using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
//using Gyroscope = UnityEngine.InputSystem.Gyroscope; Use this if you need to use gyroscope.
public class InputManager : MonoBehaviour
{
    public Vector3 gravityOrientationRawData; // Orientation of the mobile device relative to gravity
    public double rotationCutOffLimit = 0.3; // Cut off limit for when the slime should be applied with a force
    public static event Action<float> onGravityApply; // Public instance so other classes can apply methods to action event
    void Start()
    {
        SensorCheck();
        enabled = false;
        Invoke("Enabler", 1.5f);

    }
    void Enabler()
    {
        enabled = true;
    }

    void Update()
    {
        SensorCheck();

        //gravityOrientationRawData = GravitySensor.current.gravity.ReadValue(); //REMEMBER TO UNCOMMENT THIS TO ENABLE CONTROL
        // Gravity is equal to gravity sensor. 
        // The value (0, -1, 0) would be the same as holding the phone perfectly upright in your hand.
    }

    void FixedUpdate(){
        ApplyGravity(); // In FixedUpdate because it's going to handle physics 
    }

    
    void SensorCheck(){ //Checks if sensor is null, otherwise enables sensor.
        if (GravitySensor.current != null)
        {
            EnableSensor();
        }
        else {
            return;
        }
    }

    void EnableSensor(){
        InputSystem.EnableDevice(GravitySensor.current); // Enables sensor
    }

    void ApplyGravity(){ // Calling event delegate

        float orientationXValue = Math.Abs(gravityOrientationRawData.x);
        // The rotation of holding phone vertically. 
        // Whole number so it accounts for the phone in both directions.
        float phoneXValue = gravityOrientationRawData.x;

        if(orientationXValue > rotationCutOffLimit){
            onGravityApply?.Invoke(phoneXValue);
        }
    }
}