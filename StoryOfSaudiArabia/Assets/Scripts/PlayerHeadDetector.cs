using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHeadDetector : MonoBehaviour
{
    private Rigidbody2D playerRb;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip woodBreakSound;
    private AudioSource audioSource;

    private void Awake()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerRb.linearVelocity.y > 0.1f)
        {
            DestructibleBlock block = collision.GetComponent<DestructibleBlock>();

            if (block != null)
            {
                if (woodBreakSound != null)
                {
                    audioSource.PlayOneShot(woodBreakSound);
                }

                block.OnHitByHead();
            }
        }
    }
}