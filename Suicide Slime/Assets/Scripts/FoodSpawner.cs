using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public FoodPool foodPool;
    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating("SpawnFood", 0f, spawnInterval);
    }

    void SpawnFood()
    {
        GameObject food = foodPool.GetPooledObject();
        if (food != null)
        {
            food.transform.position = GetRandomPosition();
            food.SetActive(true);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-5f, 5f);
        return new Vector3(x, y, 0f);
    }
}