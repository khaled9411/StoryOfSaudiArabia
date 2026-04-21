using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<int> OnHealthChanged;

    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Invincibility Settings")]
    public float invincibleDuration = 1.5f;
    private bool isInvincible = false;
    private bool isDead = false;

    [Header("Death Settings")]
    public float fallDeathYThreshold = -10f;
    public float deathBounceForce = 12f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        OnHealthChanged?.Invoke(currentHealth);
    }

    void Update()
    {
        if (!isDead && transform.position.y < fallDeathYThreshold)
        {
            Die();
        }
    }

    public void TakeDamage()
    {
        if (isInvincible || isDead) return;

        currentHealth--;

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            StartInvincibility();
        }
    }

    private void StartInvincibility()
    {
        isInvincible = true;

        sr.DOFade(0.2f, 0.15f).SetLoops(-1, LoopType.Yoyo).SetId(this);

        Invoke(nameof(StopInvincibility), invincibleDuration);
    }

    private void StopInvincibility()
    {
        isInvincible = false;
        DOTween.Kill(this);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        DOTween.Kill(this);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);

        anim.SetBool("die", true);

        col.enabled = false;

        rb.linearVelocity = new Vector2(0, deathBounceForce);
        GameManager.Instance.TriggerDeath();
    }
}