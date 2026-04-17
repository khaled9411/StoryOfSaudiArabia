using UnityEngine;

public class DestructibleBlock : MonoBehaviour
{
    public enum BlockType { Normal, Special }

    [Header("Block Settings")]
    [SerializeField] private BlockType type = BlockType.Normal;
    [SerializeField] private GameObject breakEffect;

    public void OnHitByHead()
    {
        if (type == BlockType.Normal)
        {
            DestroyBlock();
        }
        else if (type == BlockType.Special)
        {
            HandleSpecialBlock();
        }
    }

    private void DestroyBlock()
    {
        if (breakEffect != null) Instantiate(breakEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void HandleSpecialBlock()
    {

    }
}