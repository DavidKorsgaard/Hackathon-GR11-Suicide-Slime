using UnityEngine;

public class Slime : MonoBehaviour
{
    private new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void ChangeColor(Color color)
    {
        renderer.material.color = color;
    }
}