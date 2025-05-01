using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class Slime : MonoBehaviour
{
    //Mig der prøver at finde på det modsatte af hunger :(
    [SerializeField] private int maxSatiety;
    private int satiety;
    private SpriteShapeRenderer spriteRenderer;
    [SerializeField] private float hungerRate =1;
    private float hungerTime;
    private float forceSize = 100f;
    Rigidbody2D slimeRigidbody;
    private bool gameOver;
    
    void Start()
    {
        Debug.Log("Slime Start");
        satiety = maxSatiety;
        spriteRenderer = GetComponentInChildren<SpriteShapeRenderer>();
        InputManager.onGravityApply += slimeFall;
        slimeRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
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
            KillSlime("hunter");
        }

        if (gameOver)
        {
            if (1==1)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                break;
            
            case "OoB":
                break;
        }
        
        //This is a great example of a magic number and is purely one for demonstrative reasons
        Time.timeScale = 0.3f;
        gameOver = true;
    }

    void slimeFall(float phoneXValue)
    {
        if(phoneXValue > 0){
            Debug.Log("right");
            slimeRigidbody.AddForce(Vector2.right * forceSize);
        }
        if(phoneXValue < 0){
            Debug.Log("left");
            slimeRigidbody.AddForce(Vector2.left * forceSize);
        }
    }
}