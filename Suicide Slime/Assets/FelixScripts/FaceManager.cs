using UnityEngine;

public class FaceManager : MonoBehaviour
{
    public GameObject spawnFoodFace;
    public GameObject dragFoodFace;
    public GameObject sadFace;
    public GameObject happyFace;
    private Slime slime;
    private bool showSpawnFoodFace = false;
    private float spawnFoodFaceTimer = 0f;
    public float spawnFoodFaceDuration = 1f; // Duration to show spawnFoodFace
    public AudioClip[] HappySounds;
    public AudioClip[] SadSounds;
    public AudioClip[] HungrySounds;

    private AudioSource audioSource; // Audio source component

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        slime = FindObjectOfType<Slime>();
    }

    void Update()
    {
        UpdateFace();
    }

    void UpdateFace()
    {
        if (showSpawnFoodFace)
        {
            spawnFoodFaceTimer += Time.deltaTime;
            if (spawnFoodFaceTimer >= spawnFoodFaceDuration)
            {
                showSpawnFoodFace = false;
                spawnFoodFaceTimer = 0f;
                ActivateFaceBasedOnSatiety();
            }
            else
            {
                ActivateFace(spawnFoodFace);

            }
            
        }
        else if (FoodBeingDragged())
        {
            ActivateFace(dragFoodFace);
           
        }
        else
        {
            ActivateFaceBasedOnSatiety();
            
        }
    }

    public void ShowSpawnFoodFace()
    {
        showSpawnFoodFace = true;
        spawnFoodFaceTimer = 0f;
        ActivateFace(spawnFoodFace);
    }

    void ActivateFaceBasedOnSatiety()
    {
        if (slime.GetSatiety() < slime.GetMaxSatiety() / 2)
        {
            ActivateFace(sadFace);
        }
        else
        {
            ActivateFace(happyFace);
        }
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
