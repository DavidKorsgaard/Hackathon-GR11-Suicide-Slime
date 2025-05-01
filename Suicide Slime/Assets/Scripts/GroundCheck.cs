using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public LayerMask groundLayer; // Layer to check against
    public AudioClip[] groundAudioClips; // Array of audio clips to play when grounded
    private AudioSource audioSource; // Audio source component
    public CircleCollider2D circleCollider; // Circle Collider 2D component

    private bool wasGrounded; // Track the previous grounded state

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();
        Debug.Log("GroundCheck script started.");
    }

    void Update()
    {
        bool isGrounded = IsGrounded();

        if (isGrounded && !wasGrounded)
        {
            // Play a random audio clip from the array when the character becomes grounded
            PlayRandomGroundAudioClip();
        }

        wasGrounded = isGrounded;
        
    }

    bool IsGrounded()
    {
        // Check if the Circle Collider is touching the ground layer
        bool hitGround = circleCollider.IsTouchingLayers(groundLayer);
        
        return hitGround;
    }

    void PlayRandomGroundAudioClip()
    {
        if (groundAudioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, groundAudioClips.Length);
            Debug.Log("Playing audio clip: " + groundAudioClips[randomIndex].name);
            audioSource.PlayOneShot(groundAudioClips[randomIndex]);
        }
        else
        {
            Debug.LogWarning("No audio clips assigned to groundAudioClips array.");
        }
    }
}