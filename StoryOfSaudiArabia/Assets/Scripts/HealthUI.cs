using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Image[] hearts;

    [Header("Animation Settings")]
    public float animationDuration = 0.3f;

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHeartsUI;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHeartsUI;
        }
    }

    private void UpdateHeartsUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                if (hearts[i].transform.localScale.x < 1f)
                {
                    hearts[i].transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);
                }
            }
            else
            {
                if (hearts[i].transform.localScale.x > 0f)
                {
                    Sequence loseHeartSequence = DOTween.Sequence();

                    loseHeartSequence.Append(hearts[i].transform.DOShakeRotation(0.2f, new Vector3(0, 0, 30f)));
                    loseHeartSequence.Append(hearts[i].transform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack));
                }
            }
        }
    }
}