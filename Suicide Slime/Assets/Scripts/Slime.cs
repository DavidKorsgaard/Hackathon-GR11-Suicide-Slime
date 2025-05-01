using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using Random = System.Random;

public class Slime : MonoBehaviour
{
    //Mig der prøver at finde på det modsatte af hunger :(
    [SerializeField] private int maxSatiety;
    private int satiety;
    private SpriteShapeRenderer spriteRenderer;
    [SerializeField] private float hungerRate =1; /* s/nutrient */
    private float hungerTime;
    private float forceSize = 30f;
    Rigidbody2D slimeRigidbody;
    private bool gameOver;
    private ActionController actionController;
    
    void Start()
    {
        Debug.Log("Slime Start");
        satiety = maxSatiety;
        actionController = GetComponent<ActionController>();
        spriteRenderer = GetComponentInChildren<SpriteShapeRenderer>();
        InputManager.onGravityApply += slimeFall; // slimeFall method is applied to onGravityApply action event
        slimeRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!gameOver)
        {
            hungerTime += Time.deltaTime;
            if (hungerTime >= hungerRate)
            {
                hungerTime -= hungerRate;
                satiety--;
                Debug.Log("Satiety: " + satiety);
            }

            if (satiety <= 0)
            {
                Debug.Log("Slime dead");
                KillSlime("hunger");
            }
        }
    }

    public void FeedSlime(Color color, int nutrition)
    {
        Debug.Log("Feed Slime!");
        satiety += nutrition;
        if (satiety > maxSatiety)
        {
            satiety = maxSatiety;
        }
        Debug.Log("New Satiety: " + satiety);
        spriteRenderer.color = color;
    }

    public virtual void KillSlime(string deathType)
    {
        switch (deathType)
        {
            case "lmao":
                //dies
                Destroy(gameObject);
                break;
            
            case "hunger":
                SpringJoint2D[] temp = this.GetComponentsInChildren<SpringJoint2D>();
                Debug.Log("Joint count: " + temp.Length);
                for (int i = 0; i < temp.Length; i++)
                {
                    if (UnityEngine.Random.Range(0f,1f) > 0.5f)
                    {
                        temp[i].enabled = false;
                    }
                }
                actionController.alive = false;
                break;
            
            case "OoB":
                
                break;
        }
        
        //This is a great example of a magic number and is purely there for demonstrative reasons
        //Time.timeScale = 0.3f;
        gameOver = true;
        StartCoroutine(waitOnReload());
    }

    void slimeFall(float phoneXValue) 
    {
        if(phoneXValue > 0){ // If phone is leaning right move slime right
            slimeRigidbody.AddForce(Vector2.right * phoneXValue * forceSize);
        }
        if(phoneXValue < 0){ // If phone is leaning left move slime right
            slimeRigidbody.AddForce(Vector2.left * Math.Abs(phoneXValue) * forceSize);
        }
    }
    
    IEnumerator waitOnReload()
    {
        //Watch this incredible        magic       number
        yield return new WaitForSeconds(10/Time.timeScale);
        Time.timeScale = 1;
        gameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}