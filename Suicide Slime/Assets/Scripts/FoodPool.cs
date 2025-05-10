using System.Collections.Generic;
using UnityEngine;

public class FoodPool : MonoBehaviour
{
    public GameObject[] foodPrefabs; // Make sure these prefabs have the appropriate Food child classes attached
    private List<GameObject> pooledObjects;
    public int poolSize = 10;

    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            foreach (var prefab in foodPrefabs)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                
                // Add required components if they don't exist
                if (obj.GetComponent<Rigidbody2D>() == null)
                {
                    obj.AddComponent<Rigidbody2D>();
                }
                
                // Check if any Food component exists - if not, log a warning
                Food foodComponent = obj.GetComponent<Food>();
                if (foodComponent == null)
                {
                    Debug.LogWarning("Food prefab doesn't have a Food-derived component attached: " + prefab.name);
                }
                
                // Add collider if none exists
                if (obj.GetComponent<Collider2D>() == null)
                {
                    obj.AddComponent<BoxCollider2D>();
                }
                
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}