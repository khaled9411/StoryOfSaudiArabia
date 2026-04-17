using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    private Vector3 targetPoint;
    private bool isDead = false;

    [Header("Components")]
    public Animator anim;
    public Collider2D col;
    public SpriteRenderer sr;

    void Start()
    {
        targetPoint = pointB.position;
    }

    void Update()
    {
        if (isDead) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            targetPoint = targetPoint == pointA.position ? pointB.position : pointA.position;
            sr.flipX = !(targetPoint.x < transform.position.x);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                bool isStomp = false;
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    if (contact.normal.y <= -0.5f)
                    {
                        isStomp = true;
                        break;
                    }
                }

                if (isStomp)
                {
                    EnemyDie();
                    collision.rigidbody.linearVelocity = new Vector2(collision.rigidbody.linearVelocity.x, 8f);
                }
                else
                {
                    player.TakeDamage();
                }
            }
        }
    }

    private void EnemyDie()
    {
        isDead = true;
        col.enabled = false;
        anim.SetTrigger("die");

        Destroy(gameObject, 1f);
    }
}