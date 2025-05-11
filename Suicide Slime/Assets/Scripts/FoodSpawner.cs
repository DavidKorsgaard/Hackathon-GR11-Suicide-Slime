using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public FoodPool foodPool;
    public float spawnInterval = 5f;
    public FaceManager faceManager; // Reference to FaceManager
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;

    // List of available food types - will be populated from the pool
    private List<System.Type> foodTypes = new List<System.Type>();
    private System.Type lastFoodType = null;

    void Start()
    {
        // Find all food types from the pool
        DiscoverFoodTypes();

        // Start spawning food
        InvokeRepeating("SpawnFood", 0f, spawnInterval);
    }

    private void DiscoverFoodTypes()
    {
        // Clear previous list
        foodTypes.Clear();

        // Check if red food is available
        if (foodPool.transform.Find("RedFood"))
        {
            foodTypes.Add(typeof(RedFood));
            Debug.Log("Red food type discovered");
        }

        // Check if green food is available
        if (foodPool.transform.Find("GreenFood"))
        {
            foodTypes.Add(typeof(GreenFood));
            Debug.Log("Green food type discovered");
        }

        // Check if blue food is available
        if (foodPool.transform.Find("BlueFood"))
        {
            foodTypes.Add(typeof(BlueFood));
            Debug.Log("Blue food type discovered");
        }

        // Shuffle the list for initial randomness
        ShuffleList(foodTypes);

        if (foodTypes.Count == 0)
        {
            Debug.LogWarning("No food types found in the pool! Make sure your food pool contains food objects.");
        }
        else
        {
            Debug.Log("Found " + foodTypes.Count + " food types for spawning.");
        }
    }

    void SpawnFood()
    {
        GameObject food = GetNextFood();

        if (food != null)
        {
            food.transform.position = GetRandomPosition();
            food.SetActive(true);

            if (faceManager != null)
            {
                faceManager.ShowSpawnFoodFace(); // Notify FaceManager
            }
        }
    }

    GameObject GetNextFood()
    {
        if (foodTypes.Count == 0)
        {
            Debug.LogWarning("No food types available!");
            return foodPool.GetPooledObject(); // Fallback to original method
        }

        // Ensure the next food type is different from the last one
        System.Type nextFoodType = null;
        do
        {
            nextFoodType = foodTypes[Random.Range(0, foodTypes.Count)];
        } while (nextFoodType == lastFoodType);

        lastFoodType = nextFoodType;
        return foodPool.GetPooledObjectOfType(nextFoodType);
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector3(x, y, 0f);
    }

    // Helper method to shuffle a list
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[random];
            list[random] = temp;
        }
    }
}
