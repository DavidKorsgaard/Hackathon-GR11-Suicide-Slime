using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private Rigidbody2D rb; //Variable rb of type Rigidbody2D is used to manipulate physics of the object

    private System.Action lastAction;

    //SerializeField is used so we can adjust the values in the unity inspector
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float delayBetweenActions = 5f;

    [SerializeField] List<Rigidbody2D> allrigidbodies;  //

    private bool hasLanded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D on the cube
        StartCoroutine(RandomActions());  // Start random action loop
    }

    public void RegisterLanding()  //Method called when object its attached to hit object with "ground" tag
    {
        if (!hasLanded && (lastAction == JumpUp || lastAction == JumpLeft || lastAction == JumpRight))   //Triggers if object has not landed and if the last action was a jump
        {
            Debug.Log("Slime has Landed, stopping movement");

            foreach (var body in allrigidbodies)  //Stops the movement of objects
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = 0f;
            }

            hasLanded = true;                     //Movement doesnt stop again until another jump action has happened
        }
    }

    void MoveLeft()
    {
        rb.AddForce(Vector3.left * moveSpeed, ForceMode2D.Impulse); // Change the object's linear velocity on x-axis to move left when method is called
        Debug.Log("MoveLeft"); // ForceMode2D.Impulse tells Unity how to apply it; Impulse gives it an instant push
    }

    void MoveRight()
    {
        rb.AddForce(Vector3.right * moveSpeed, ForceMode2D.Impulse); // Change the object's linear velocity on x-axis to move right
        Debug.Log("MoveLeft");
    }

    void JumpUp()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse); // AddForce applies upward force to the object
        Debug.Log("JumpUp");
    }

    void JumpRight()
    {
        rb.AddForce((Vector3.up + Vector3.right * jumpSpeed) * jumpForce, ForceMode2D.Impulse); // Diagonal up-right jump
        Debug.Log("JumpRight");
    }

    void JumpLeft()
    {
        rb.AddForce((Vector3.up + Vector3.left * jumpSpeed) * jumpForce, ForceMode2D.Impulse); // Diagonal up-left jump
        Debug.Log("JumpLeft");
    }

    IEnumerator RandomActions() // Coroutine that runs a random set of actions with a delay between each
    {
        while (true)
        {
            List<System.Action> actions = new List<System.Action>
            {
                MoveLeft,
                MoveRight,
                JumpUp,
                JumpRight,
                JumpLeft
            };

            Shuffle(actions); // Shuffles list randomly with the Shuffle<T>() method

            foreach (var action in actions)
            {
                lastAction = action;
                hasLanded = false; //hasLanded resets when new action is selected
                action.Invoke(); // Invokes the action
                yield return new WaitForSeconds(delayBetweenActions); // Delay between actions
            }
        }
    }

    void Shuffle<T>(List<T> list) // Generic shuffle method using Fisher-Yates shuffle
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lastAction == JumpLeft || lastAction == JumpRight || lastAction == JumpUp)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = 0f;
            Debug.Log("Stopped After Jumping");
        }
    }

    private void FixedUpdate()
    {

    }
}