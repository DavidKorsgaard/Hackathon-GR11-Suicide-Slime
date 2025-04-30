using UnityEngine;

public class Food : MonoBehaviour
{
    public int nutritionalValue;
    private bool isDragging = false;
    private Vector3 offset;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>(); // Add Collider2D component if not already present
        }
        GetComponent<Collider2D>().isTrigger = true; // Ensure the collider is set to trigger
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition + offset;
            rb.isKinematic = true; // Disable gravity while dragging
        }
        else
        {
            rb.isKinematic = false; // Enable gravity when not dragging
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = transform.position - mousePosition;
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckIfFedToSlime();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D called with: " + collision.collider.name);
        if (collision.collider.CompareTag("Slime"))
        {
            Slime slime = collision.collider.GetComponent<Slime>();
            FeedSlime(slime);
            gameObject.SetActive(false);
            FindObjectOfType<FoodPool>().ReturnToPool(gameObject);
        }

    }

    void CheckIfFedToSlime()
    {
        Collider2D slimeCollider = FindObjectOfType<Slime>().GetComponent<Collider2D>();
        if (slimeCollider.bounds.Contains(transform.position))
        {
            Slime slime = slimeCollider.GetComponent<Slime>();
            FeedSlime(slime);
            gameObject.SetActive(false);
            FindObjectOfType<FoodPool>().ReturnToPool(gameObject);
        }
    }

    void FeedSlime(Slime slime)
    {
        slime.FeedSlime(GetComponent<Renderer>().material.color, nutritionalValue);
    }

    public virtual void ChangeSlimeColor(Slime slime) { }
}

public class RedFood : Food
{
    void Start()
    {
        nutritionalValue = 10; // Set nutritional value for RedFood
    }

    public override void ChangeSlimeColor(Slime slime)
    {
        slime.ChangeColor(Color.red);
    }
}

public class BlueFood : Food
{
    void Start()
    {
        nutritionalValue = 20; // Set nutritional value for BlueFood
    }

    public override void ChangeSlimeColor(Slime slime)
    {
        slime.ChangeColor(Color.blue);
    }
}

// Add more food classes as needed
