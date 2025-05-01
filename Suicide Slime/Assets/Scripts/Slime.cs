using System;
using UnityEngine;
using UnityEngine.U2D;

public class Slime : MonoBehaviour
{
    //Mig der prøver at finde på det modsatte af hunger :(
    [SerializeField] private int maxSatiety;
    private int satiety;
    private SpriteShapeRenderer spriteRenderer;
    [SerializeField] private float hungerRate =1;
    private float hungerTime;
    private float forceSize = 30f;
    Rigidbody2D slimeRigidbody;    
    void Start()
    {
        Debug.Log("Slime Start");
        satiety = maxSatiety;
        spriteRenderer = GetComponentInChildren<SpriteShapeRenderer>();
        InputManager.onGravityApply += slimeFall; // slimeFall method is applied to onGravityApply action event
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
            KillSlime("lmao");
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
        
        Destroy(gameObject);
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
}