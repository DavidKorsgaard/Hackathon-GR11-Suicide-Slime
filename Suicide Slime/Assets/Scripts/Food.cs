using UnityEngine;

public abstract class Food : MonoBehaviour
{
    [SerializeField] protected int nutritionalValue;
    [SerializeField] protected Color foodColor = Color.magenta;
    private bool isDragging = false;
    private Vector3 offset;
    private Rigidbody2D rb;
    private Collider2D collider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<Collider2D>() == null)
        {
            collider = gameObject.AddComponent<CircleCollider2D>(); // Add Collider2D if not already present
        }
        else
        {
            collider = GetComponent<Collider2D>();
        }
        
        InitializeFood(); // Call new method for child classes to initialize their values
    }

    // New virtual method for child classes to override
    protected virtual void InitializeFood()
    {
        // Base implementation does nothing - will be overridden by child classes
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition + offset;
            rb.bodyType = RigidbodyType2D.Kinematic; // Disable gravity while dragging
            collider.enabled = false;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Enable gravity when not dragging
            collider.enabled = true;
        }
    }

    void OnMouseDown() //Events that happen when you touch food
    {
        isDragging = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = transform.position - mousePosition;
    }

    void OnMouseUp() //Events that happen when you let go of food
    {
        isDragging = false;
        CheckIfFedToSlime(); //If not fed to slime, gravity is enabled and food will fall
    }
    
    void CheckIfFedToSlime() //Method to return food to object pool if fed to slime
    {
        CircleCollider2D slimeCollider = FindObjectOfType<Slime>().GetComponent<CircleCollider2D>();
        if (slimeCollider.bounds.Contains(transform.position))
        {
            Slime slime = slimeCollider.GetComponent<Slime>();
            FeedSlime(slime); // Call our new method instead
            gameObject.SetActive(false);
            FindObjectOfType<FoodPool>().ReturnToPool(gameObject);
        }
    }

    // New method to feed the slime - calls the implementation in the child class
    public void FeedSlime(Slime slime)
    {
        // Increase satiety using the nutritionalValue
        slime.UpdateSatiety(nutritionalValue);
        
        // Change color using the child class implementation
        ChangeSlimeColor(slime);
        
        // Change the slime shape using the ShapeManager
        ShapeManager shapeManager = slime.GetComponent<ShapeManager>();
        if (shapeManager != null)
        {
            shapeManager.ChangeShape(this.GetType());
        }
    }

    // Abstract method that must be implemented by child classes
    public abstract void ChangeSlimeColor(Slime slime);

    // Add this method to check if the food is being dragged
   /* public bool IsDragging()
    {
        return isDragging;
    }
    */
}