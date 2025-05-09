using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private float forceSize = 30f;
    [SerializeField] Rigidbody2D slimeRigidbody;
    private bool gameOver;
    private bool controller = true;
    private ActionController actionController;

    public GameObject slimeCanvas;
    public GameObject fallingSlime;
    public GameObject BackgorundSound;
    public GameObject foodManager;
    public GameObject table;
    

    void Awake()
    {
        slimeRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Debug.Log("Slime Start");
        satiety = maxSatiety;
        actionController = GetComponent<ActionController>();
        spriteRenderer = GetComponentInChildren<SpriteShapeRenderer>();
        InputManager.onGravityApply += slimeFall; // slimeFall method is applied to onGravityApply action event
        


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

        if (transform.position.y < -10)
        {
            KillSlime("OoB");
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
        color.a = 0.647f;
        spriteRenderer.color = color;

    }

    public virtual void KillSlime(string deathType)
    {
        switch (deathType)
        {
            case "lmao":
                //dies
                
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
                StartCoroutine(WaitOnHunger());
                controller = false;
                break;
            
            case "OoB":
                
                break;
        }

        //This is a great example of a magic number and is purely there for demonstrative reasons
        //Time.timeScale = 0.3f;

        if (controller == false)
        {
            StartCoroutine(WaitOnHunger());
        }
        else
        {
            StartCoroutine(waitOnReload());
        }
            
        
        gameOver = true;
        
        
        
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
    IEnumerator WaitOnHunger()
    {
        yield return new WaitForSeconds(8 / Time.timeScale);
        Time.timeScale = 1;
        gameOver = false;
        InputSystem.DisableDevice(GravitySensor.current);
        SceneManager.LoadScene(0);
    }

    IEnumerator waitOnReload()
    {
        table.SetActive(false);
        slimeCanvas.SetActive(true);
        fallingSlime.SetActive(true);
        BackgorundSound.SetActive(false);
        this.gameObject.SetActive(false);
        foodManager.SetActive(false);

        
        //Watch this incredible        magic       number
        yield return new WaitForSeconds(13/Time.timeScale);
        gameOver = false;
        InputSystem.DisableDevice(GravitySensor.current);

        SceneManager.LoadScene(0);
    }

    public int GetSatiety()
    {
        return satiety;
    }

    public int GetMaxSatiety()
    {
        return maxSatiety;
    }

}