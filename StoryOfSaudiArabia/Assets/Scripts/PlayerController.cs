using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int maxJumps = 2;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;

    private float horizontalInput = 0f;
    private bool isGrounded;
    private int jumpsRemaining;
    private bool isDead = false;

    private readonly int isMovingHash = Animator.StringToHash("isMoving");
    private readonly int dieHash = Animator.StringToHash("die");
    private readonly int jumpingHash = Animator.StringToHash("jumping");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        if (isDead) return;

        CheckStatus();
        UpdateAnimations();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void CheckStatus()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpsRemaining = maxJumps;
        }
    }

    private void UpdateAnimations()
    {
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;
        animator.SetBool(isMovingHash, isMoving);
    }

    private void FlipSprite()
    {
        if (horizontalInput > 0)
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
    }

    #region Mobile UI Input Methods

    public void MoveRightDown() { if (!isDead) horizontalInput = 1f; }
    public void MoveLeftDown() { if (!isDead) horizontalInput = -1f; }
    public void StopMoving() { if (!isDead) horizontalInput = 0f; }

    public void Jump()
    {
        if (isDead) return;

        if (jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            animator.SetTrigger(jumpingHash);
            jumpsRemaining--;
        }
    }

    #endregion

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        horizontalInput = 0f;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool(dieHash, true);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}