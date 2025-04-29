using UnityEngine;

public class Food : MonoBehaviour

{
    private bool isDragging = false;
    private Vector3 offset;
    
    public virtual void ChangeSlimeColor(Slime slime) { }

    void Update()
    {
        if (isDragging)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            transform.position = touchPosition + offset;
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckIfFedToSlime();
    }

    void CheckIfFedToSlime()
    {
        Collider2D slimeCollider = FindObjectOfType<Slime>().GetComponent<Collider2D>();
        if (slimeCollider.bounds.Contains(transform.position))
        {
            slimeCollider.GetComponent<Slime>().ChangeColor(GetComponent<Renderer>().material.color);
            gameObject.SetActive(false);
            FindObjectOfType<FoodPool>().ReturnToPool(gameObject);
        }
    }
}

public class RedFood : Food
{
    public override void ChangeSlimeColor(Slime slime)
    {
        slime.ChangeColor(Color.red);
    }
}

public class BlueFood : Food
{
    public override void ChangeSlimeColor(Slime slime)
    {
        slime.ChangeColor(Color.blue);
    }
}

// Add more food classes as needed
