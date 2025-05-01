using UnityEngine;

public class NewFood : MonoBehaviour
{
    public int nutritionalValue;
    [SerializeField] private Color foodColor = Color.magenta;

    private bool isDragging = false; // Tracks whether the food is being dragged
    private Vector3 offset;
    private Rigidbody2D rb;
    private Collider2D collider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure it has a Collider2D (default to CircleCollider2D)
        if (GetComponent<Collider2D>() == null)
        {
            collider = gameObject.AddComponent<CircleCollider2D>();
        }
        else
        {
            collider = GetComponent<Collider2D>();
        }
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition + offset;
            rb.bodyType = RigidbodyType2D.Kinematic;  // Disable gravity during drag
            collider.enabled = false;  // Disable the collider during drag
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;  // Enable gravity when not dragging
            collider.enabled = true;  // Re-enable the collider
        }
    }

    void OnMouseDown()
    {
        isDragging = true;  // Start dragging
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = transform.position - mousePosition;  // Set the offset for dragging
    }

    void OnMouseUp()
    {
        isDragging = false;  // Stop dragging
        CheckIfFedToSlime();  // Check if food has been fed to the slime
    }

    void CheckIfFedToSlime()
    {
        NewSlime slime = FindObjectOfType<NewSlime>();
        if (slime == null)
        {
            Debug.LogWarning("No NewSlime found in scene.");
            return;
        }

        CircleCollider2D slimeCollider = slime.GetComponent<CircleCollider2D>();
        if (slimeCollider.bounds.Contains(transform.position))
        {
            slime.FeedSlime(foodColor, nutritionalValue);
            gameObject.SetActive(false);  // Disable the food object
            FoodPool pool = FindObjectOfType<FoodPool>();
            if (pool != null)
            {
                pool.ReturnToPool(gameObject);  // Return the food object to the pool
            }
            else
            {
                Destroy(gameObject);  // Fallback if no pool found
            }
        }
    }

    // New method to check if food is being dragged
    public bool IsDragging()
    {
        return isDragging;  // Return the current dragging state
    }
}
