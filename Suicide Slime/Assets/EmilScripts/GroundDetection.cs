using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    private ActionController actionController;  //Variable that references ActionController script

    private void Start()
    {
        actionController = GetComponent<ActionController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)  //Runs on attched gameobjects collision
    {
        if (collision.collider.CompareTag("Ground"))        //Checks if collided object hads the tag "Ground"
        {
            actionController.RegisterLanding();             //When Ground collision is detected Registerlanding method is called from ActionController script
        }

    }
}
