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
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            transform.position = touchPosition + offset;
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
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckIfFedToSlime();
    }

    void CheckIfFedToSlime()
    {
        Collider2D slimeCollider = FindObjectOfType<Slime>().GetComponent<Collider2D>();
        if (slimeCollider.bounds.Contains(transform.position))
        {
            ChangeSlimeColor(slimeCollider.GetComponent<Slime>());
            gameObject.SetActive(false);
            FindObjectOfType<FoodPool>().ReturnToPool(gameObject);
        }
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
        nutritionalValue = 15; // Set nutritional value for BlueFood
    }

    public override void ChangeSlimeColor(Slime slime)
    {
        slime.ChangeColor(Color.blue);
    }
}

public class GreenFood : Food
{
    void Start()
    {
        nutritionalValue = 20; // Set nutritional value for BlueFood
    }

    public override void ChangeSlimeColor(Slime slime)
    {
        slime.ChangeColor(Color.green);
    }
}
