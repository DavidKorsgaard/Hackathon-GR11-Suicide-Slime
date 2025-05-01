using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ShrinkSoundController : MonoBehaviour
{
    public AudioClip shrinkSound;
    public AudioClip popSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("WaitOnReload", 12f);
        
    }

    // Call this at the *start* of the shrink animation
    public void PlayShrinkSound()
    {
        audioSource.clip = shrinkSound;
        audioSource.Play();
    }

    // Call this at the *end* of the shrink animation
    public void PlayPopSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(popSound);
    }
    public void WaitOnReload()
    {
        SceneManager.LoadScene(0);
    }
}