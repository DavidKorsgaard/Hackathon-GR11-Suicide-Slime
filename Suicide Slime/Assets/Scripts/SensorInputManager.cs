using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
//using Gyroscope = UnityEngine.InputSystem.Gyroscope; Use this if you need to use gyroscope.
public class InputManager : MonoBehaviour
{
    public Vector3 gravityOrientationRawData; // Orientation of mobile device relative to gravity
    public double rotationCutOffLimit = 0.3; // Cut off limit for when the slime shold be applied a force
    public static event Action<float> onGravityApply; // Public instance so other classes can apply methods to action event
    void Start()
    {
        SensorStartCheck(); // Reenables sensor in case the system has not registered it yet
    }

    void Update()
    {
        SensorCheck();

        gravityOrientationRawData = GravitySensor.current.gravity.ReadValue(); 
        // Gravity is equal to gravity sensor. 
        // The value (0, -1, 0) would be the same as holding the phone perfectly upright in your hand.
    }

    void FixedUpdate(){
        ApplyGravity(); // In fixedupdate because it's going to handle physics 
    }

    IEnumerator SensorStartCheck()  // IEnumerator is returntype
    {
        yield return new WaitForSeconds(3f); // Returns coroutine and waits three second before checking if sensor is enabled
        SensorCheck();
    }

    void SensorCheck(){ //Checks if sensor is null. Otherwise enables sensor.
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