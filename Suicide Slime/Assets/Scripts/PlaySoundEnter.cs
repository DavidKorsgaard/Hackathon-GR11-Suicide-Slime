using UnityEngine;

public class Play : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0,1)] private float volume = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.PlaySound(sound, volume);  
    }
}
