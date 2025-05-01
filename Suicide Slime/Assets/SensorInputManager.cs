using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
//using Gyroscope = UnityEngine.InputSystem.Gyroscope; Use this if you need to use gyroscope.
public class InputManager : MonoBehaviour
{
    public Vector3 gravityOrientation; // Orientation of mobile device relative to gravity
    public double rotationCutOffLimit = 0.6; // Cut off limit for when the slime shold be applied a force
    public delegate void OnGravityApply(); // Delegate type for triggering gravity to be applied
    public static event OnGravityApply onGravityApply; // Public instance so other classes can apply methods to event delegate
    void Start()
    {
        EnableSensor(); // Enables sensor

        SensorStartCheck(); // Reenables sensor in case the system has not registered it yet
    }

    void Update()
    {
        SensorCheck();

        gravityOrientation = GravitySensor.current.gravity.ReadValue(); 
        // Gravity is equal to gravity sensor. 
        // The value (0, -1, 0) would be the same as holding the phone perfectly upright in your hand.

        Debug.Log(gravityOrientation);
    }

    void FixedUpdate(){
        ApplyGravity(); // In fixedupdate because it's going to handle physics 
    }

    IEnumerator SensorStartCheck()  // IEnumerator is returntype
    {
        yield return new WaitForSeconds(1f); // Returns coroutine and waits a second before checking if sensor is enabled
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

        float orientation = Math.Abs(gravityOrientation.x);

        if(orientation < rotationCutOffLimit){
            onGravityApply?.Invoke();
        }
    }

}