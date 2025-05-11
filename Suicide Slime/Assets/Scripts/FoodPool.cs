using System.Collections.Generic;
using UnityEngine;

public class FoodPool : MonoBehaviour
{
    public GameObject redFoodPrefab;
    public GameObject greenFoodPrefab;
    public GameObject blueFoodPrefab;
    public int poolSize = 10;

    private List<GameObject> pooledObjects = new List<GameObject>();

    void Start()
    {
        // Create the initial pool of objects
        for (int i = 0; i < poolSize; i++)
        {
            // Create one of each food type evenly
            int foodType = i % 3;
            GameObject prefab;
            string name;

            switch (foodType)
            {
                case 0:
                    prefab = redFoodPrefab;
                    name = "RedFood";
                    break;
                case 1:
                    prefab = greenFoodPrefab;
                    name = "GreenFood";
                    break;
                default:
                    prefab = blueFoodPrefab;
                    name = "BlueFood";
                    break;
            }

            GameObject obj = Instantiate(prefab, transform);
            obj.name = name;
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }

        // Shuffle the pool initially
        ShuffleList(pooledObjects);
    }

    // Gets any available pooled object
    public GameObject GetPooledObject()
    {
        // First, try to find an inactive object
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        // If no inactive objects found, return the first one (even if active)
        // This could be improved by creating more objects when needed
        if (pooledObjects.Count > 0)
        {
            Debug.LogWarning("No inactive objects in pool. Reusing active object.");
            return pooledObjects[0];
        }

        Debug.LogError("No objects in pool!");
        return null;
    }

    // Gets a specific type of food from the pool
    public GameObject GetPooledObjectOfType(System.Type foodType)
    {
        // First, try to find an inactive object of the requested type
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            Food food = pooledObjects[i].GetComponent<Food>();
            if (!pooledObjects[i].activeInHierarchy && food != null && food.GetType() == foodType)
            {
                return pooledObjects[i];
            }
        }

        // If no inactive objects of that type found, try to find any inactive object
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                Debug.LogWarning("No inactive " + foodType.Name + " found. Using different food type.");
                return pooledObjects[i];
            }
        }

        // Last resort: return any available object
        Debug.LogWarning("No inactive objects in pool. Using first available object.");
        return GetPooledObject();
    }

    // Return an object to the pool (deactivate it)
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
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
