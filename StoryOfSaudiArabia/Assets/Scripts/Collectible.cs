using UnityEngine;
using DG.Tweening;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType { Coin, Key }

    [Header("Collectible Settings")]
    [SerializeField] private CollectibleType type;
    [SerializeField] private int value = 1;
    [SerializeField] private GameObject collectEffect;

    [Header("DOTween Settings")]
    [SerializeField] private float hoverHeight = 0.5f;
    [SerializeField] private float hoverDuration = 1f;
    [SerializeField] private float shrinkDuration = 0.3f;

    private bool isCollected = false;
    private Tween hoverTween;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        hoverTween = transform.DOMoveY(transform.position.y + hoverHeight, hoverDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            AnimateCollection();
        }
    }

    private void AnimateCollection()
    {
        if (col != null) col.enabled = false;

        hoverTween.Kill();

        transform.DOScale(Vector3.zero, shrinkDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                ExecuteCollectionLogic();
            });
    }

    private void ExecuteCollectionLogic()
    {
        switch (type)
        {
            case CollectibleType.Coin:
                PlayerDataManager.Instance.AddCoins(value);
                break;
            case CollectibleType.Key:
                PlayerDataManager.Instance.AddKeys(value);
                break;
        }

        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}