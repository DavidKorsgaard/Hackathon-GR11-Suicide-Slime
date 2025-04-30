using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionController : MonoBehaviour
{
    private Rigidbody2D rb; //Variable rb of type Rigidbody2D is used to manipulate physics of the object

    //SerializeField is used so we can adjust the values in the unity inspector
    [SerializeField] float jumpForce = 10;
    [SerializeField] float moveSpeed = 5;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D on the cube
        StartCoroutine(RandomActions());  // Start random action loop
    }
    void MoveLeft()
    {
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y); // Change the objects linear velocity on x-axis to MoveLeft when method is called
    }

    void MoveRight()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y); // Change the objects linear velocity on x-axis to MoveRight when method is called
    }

    void JumpUp() 
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse); // AddForce applies force to object. 
    }                                                             //ForceMode2D.Impulse tells unity how to apply it, Impulse gives it an instant push

    void JumpRight()
    {
        rb.AddForce((Vector3.up + Vector3.right) * jumpForce, ForceMode2D.Impulse); //Same as in JumpUp but by combining Vector3.up with Vector3.right diagonally right
    }

    void JumpLeft()
    {
        rb.AddForce((Vector3.up + Vector3.left) * jumpForce, ForceMode2D.Impulse); //Same as in JumpUp but by combining Vector3.up with Vector3.left diagonally left
    }

    IEnumerator RandomActions() //Courentine that runs a random set of actions with a delay between each
    {
        while (true)
        {
            List<System.Action> actions = new List<System.Action>  // List of methods (actions) to called
            {
                MoveLeft,
                MoveRight,
                JumpUp,
                JumpRight,
                JumpLeft
            };

            Shuffle(actions);  //Shuffles list randomly with the Shuffle<T>() method

            foreach (var action in actions)  //Loops through the shuffled actions
            {
                action.Invoke();  //Invokes the action
                yield return new WaitForSeconds(3f);  //Makes the delay between actions 3 seconds
            }
        }
    }

    void Shuffle<T>(List<T> list)  // This is the shuffle method, it is a generic method which means it works with any type T(like an int or string etc.
                                   // The method shuffles the list using the Fisher-Yates shuffle algorithm
    {
        for (int i = 0; i < list.Count; i++)  //for-loop that goes through every index in list.
        {
            int rand = Random.Range(i, list.Count); // Generates random index called rand, Random.Range(i, list.Count) picks a number
            T temp = list[i];                       // Saves current item in temporary variable called temp
            list[i] = list[rand];                   // Puts randomly selected item from list[rand] into list[i] (replaces the current value with a random one)
            list[rand] = temp;                      // Swaps originaly saved item with new randomly selected one
        }
    }



    private void FixedUpdate()
    {
        
    }
}
