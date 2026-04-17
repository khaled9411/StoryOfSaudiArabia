using UnityEngine;

public class PlayerHeadDetector : MonoBehaviour
{
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerRb.linearVelocity.y > 0.1f)
        {
            DestructibleBlock block = collision.GetComponent<DestructibleBlock>();

            if (block != null)
            {
                block.OnHitByHead();
            }
        }
    }
}