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
            faceManager.ShowSpawnFoodFace(); // Notify FaceManager
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector3(x, y, 0f);
    }
}
