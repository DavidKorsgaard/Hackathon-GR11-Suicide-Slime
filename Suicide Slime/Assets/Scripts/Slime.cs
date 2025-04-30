using UnityEngine;

public class Slime : MonoBehaviour
{
    private new Renderer renderer;

    //Mig der prøver at finde på det modsatte af hunger :(
    [SerializeField] private int maxSatiety;
    private int satiety;
    [SerializeField] private float hungerRate =1;
    private float hungerTime;
    
    void Start()
    {
        Debug.Log("Slime Start");
        renderer = GetComponent<Renderer>();
        satiety = maxSatiety;
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
            //Run some kill slime method elsewhere
            Debug.Log("Slime dead");
            
        }
    }

    public void FeedSlime(Color color, int nutrition)
    {
        Debug.Log("Feed Slime!");
        satiety += nutrition;
        Debug.Log("New Satiety: " + satiety);
        renderer.material.color = color;
    }
    
    public void ChangeColor(Color color)
    {
        renderer.material.color = color;
    }

}