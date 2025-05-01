using UnityEngine;

public class FoodTracker : MonoBehaviour
{
    private NewSlime slime;  // Reference to the slime
    private NewFood food;    // Reference to the food

    void Start()
    {
        // Initialize references to NewSlime and NewFood
        slime = FindObjectOfType<NewSlime>();
        food = FindObjectOfType<NewFood>();

        if (slime == null)
        {
            Debug.LogError("No NewSlime found in the scene!");
        }

        if (food == null)
        {
            Debug.LogError("No NewFood found in the scene!");
        }
    }

    void Update()
    {
        // If food is available and being dragged, we can perform checks
        if (food != null)
        {
            bool isFoodBeingDragged = food.IsDragging();  // Check if food is being dragged

            // Example logic for when food is being dragged (could be expanded with more logic)
            if (isFoodBeingDragged)
            {
                Debug.Log("Food is being dragged!");
                // You can add more logic here based on what should happen when food is being dragged
            }
            else
            {
                Debug.Log("Food is not being dragged.");
            }
        }

        // Example logic for tracking slime hunger
        if (slime != null)
        {
            float hungerPercent = (float)slime.GetSatiety() / slime.GetMaxSatiety();  // Calculate hunger percentage
            if (hungerPercent < 0.5f)
            {
                Debug.Log("Slime is hungry!");
                // Handle slime hunger state here, like triggering events
            }
        }
    }
}
