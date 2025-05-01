using UnityEngine;

public class FaceManager : MonoBehaviour
{
    public GameObject spawnFoodFace;
    public GameObject dragFoodFace;
    public GameObject sadFace;
    public GameObject happyFace;
    private Slime slime;

    void Start()
    {
        slime = FindObjectOfType<Slime>();
    }

    void Update()
    {
        UpdateFace();
    }

    void UpdateFace()
    {
        if (FoodBeingDragged())
        {
            ActivateFace(dragFoodFace);
        }
        else if (FoodInScene())
        {
            ActivateFace(spawnFoodFace);
        }
        else if (slime.GetSatiety() < slime.GetMaxSatiety() / 2)
        {
            ActivateFace(sadFace);
        }
        else
        {
            ActivateFace(happyFace);
        }
    }

    bool FoodInScene()
    {
        return FindObjectOfType<Food>() != null;
    }

    bool FoodBeingDragged()
    {
        Food[] foods = FindObjectsOfType<Food>();
        foreach (Food food in foods)
        {
            if (food.IsDragging())
            {
                return true;
            }
        }
        return false;
    }

    void ActivateFace(GameObject face)
    {
        spawnFoodFace.SetActive(face == spawnFoodFace);
        dragFoodFace.SetActive(face == dragFoodFace);
        sadFace.SetActive(face == sadFace);
        happyFace.SetActive(face == happyFace);
    }
}
