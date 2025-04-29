using System.Collections.Generic;
using UnityEngine;

public class FoodPool : MonoBehaviour
{
    public GameObject[] foodPrefabs;
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
}