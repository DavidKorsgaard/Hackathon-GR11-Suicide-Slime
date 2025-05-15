using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using Random = System.Random;

public class Slime : MonoBehaviour
{
    [SerializeField] private int maxSatiety;
    private int satiety;
    private SpriteShapeRenderer spriteRenderer;
    [SerializeField] private float hungerRate = 1; /* s/nutrient */
    private float hungerTime;
    [SerializeField] private float forceSize = 30f;
    [SerializeField] Rigidbody2D slimeRigidbody;
    private bool gameOver;
    private bool controller = true;
    private ActionController actionController;

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
                Debug.Log("Slime dead from hunger");
                KillSlime("hunger");
            }
        }

        if (transform.position.y < -10)
        {
            KillSlime("OoB");
        }
    }

    public void UpdateSatiety(int nutrition)
    {
        satiety += nutrition;
        if (satiety > maxSatiety)
        {
            satiety = maxSatiety;
        }
        Debug.Log("New Satiety: " + satiety);
    }

    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void FeedSlime(Color color, int nutrition)
    {
        Debug.Log("Feed Slime!");
        UpdateSatiety(nutrition);
        color.a = 0.647f;
        ChangeColor(color);
    }

    public virtual void KillSlime(string deathType)
    {
        if (gameOver)
            return;
            
        gameOver = true;
        Debug.Log("Killing slime. Death type: " + deathType);

        switch (deathType)
        {
            case "lmao":
                controller = true;
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
                if (actionController != null)
                {
                    actionController.alive = false;
                    Debug.Log("Set action controller alive to false");
                }
                controller = false;
                break;
            
            case "OoB":
                controller = true;
                break;
        }

        if (controller)
        {
            Debug.Log("Starting waitOnReload coroutine");
            StartCoroutine(waitOnReload());
        }
        else
        {
            Debug.Log("Starting WaitOnHunger coroutine");
            StartCoroutine(WaitOnHunger());
        }
    }

    void slimeFall(float phoneXValue) 
    {
        if (gameOver) return;
        
        if(phoneXValue > 0){
            slimeRigidbody.AddForce(Vector2.right * phoneXValue * forceSize);
        }
        if(phoneXValue < 0){
            slimeRigidbody.AddForce(Vector2.left * Math.Abs(phoneXValue) * forceSize);  
        }
    }
    
    IEnumerator WaitOnHunger()
    {
        Debug.Log("WaitOnHunger coroutine running, waiting for " + (5 / Time.timeScale) + " seconds");
        yield return new WaitForSeconds(5 / Time.timeScale);
        Debug.Log("WaitOnHunger completed, loading scene 0");
        Time.timeScale = 1;
        //InputSystem.DisableDevice(GravitySensor.current);
        SceneManager.LoadScene(0);
    }

    IEnumerator waitOnReload()
    {
        Debug.Log("waitOnReload coroutine running, waiting for " + (1 / Time.timeScale) + " seconds");
        yield return new WaitForSeconds(1 / Time.timeScale);
        Debug.Log("waitOnReload completed, loading scene 1");
        Time.timeScale = 1;
        //InputSystem.DisableDevice(GravitySensor.current);
        SceneManager.LoadScene(1);
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
