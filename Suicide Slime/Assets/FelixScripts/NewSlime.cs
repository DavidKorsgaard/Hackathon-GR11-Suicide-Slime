using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class NewSlime : MonoBehaviour
{
    [SerializeField] private int maxSatiety;
    private int satiety;
    private SpriteShapeRenderer spriteRenderer;
    [SerializeField] private float hungerRate = 1f;
    private float hungerTime;
    [SerializeField] private float forceSize = 30f;

    private Rigidbody2D slimeRigidbody;
    private bool gameOver;
    private ActionController actionController;

    void Start()
    {
        Debug.Log("NewSlime Start");
        satiety = maxSatiety;
        actionController = GetComponent<ActionController>();
        spriteRenderer = GetComponentInChildren<SpriteShapeRenderer>();
        InputManager.onGravityApply += slimeFall;
        slimeRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!gameOver)
        {
            hungerTime += Time.deltaTime;
            if (hungerTime >= hungerRate)
            {
                hungerTime -= hungerRate;
                satiety--;
                Debug.Log("Satiety: " + satiety);
            }

            if (satiety <= 0)
            {
                Debug.Log("NewSlime dead");
                KillSlime("hunger");
            }
        }
    }

    public void FeedSlime(Color color, int nutrition)
    {
        Debug.Log("Feed NewSlime!");
        satiety += nutrition;
        if (satiety > maxSatiety)
        {
            satiety = maxSatiety;
        }
        Debug.Log("New Satiety: " + satiety);
        spriteRenderer.color = color;
    }

    public virtual void KillSlime(string deathType)
    {
        switch (deathType)
        {
            case "lmao":
                Destroy(gameObject);
                break;

            case "hunger":
                SpringJoint2D[] joints = GetComponentsInChildren<SpringJoint2D>();
                for (int i = 0; i < joints.Length; i++)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                        joints[i].enabled = false;
                }
                actionController.alive = false;
                break;

            case "OoB":
                break;
        }

        gameOver = true;
        StartCoroutine(waitOnReload());
    }

    void slimeFall(float phoneXValue)
    {
        if (phoneXValue > 0)
        {
            slimeRigidbody.AddForce(Vector2.right * phoneXValue * forceSize);
        }
        else if (phoneXValue < 0)
        {
            slimeRigidbody.AddForce(Vector2.left * Mathf.Abs(phoneXValue) * forceSize);
        }
    }

    IEnumerator waitOnReload()
    {
        yield return new WaitForSeconds(10 / Time.timeScale);
        Time.timeScale = 1;
        gameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- New for face logic ---
    public int GetSatiety()
    {
        return satiety;
    }

    public int GetMaxSatiety()
    {
        return maxSatiety;
    }
}
