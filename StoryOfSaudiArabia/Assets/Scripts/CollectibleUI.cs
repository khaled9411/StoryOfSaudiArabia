using UnityEngine;
using TMPro;

public class CollectibleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI keysText;

    private void Start()
    {
        UpdateCoinsText(PlayerDataManager.Instance.GetCoins());
        UpdateKeysText(PlayerDataManager.Instance.GetKeys());

        PlayerDataManager.Instance.OnCoinsChanged += UpdateCoinsText;
        PlayerDataManager.Instance.OnKeysChanged += UpdateKeysText;
    }

    private void OnDestroy()
    {
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.OnCoinsChanged -= UpdateCoinsText;
            PlayerDataManager.Instance.OnKeysChanged -= UpdateKeysText;
        }
    }

    private void UpdateCoinsText(int newAmount)
    {
        coinsText.text = newAmount.ToString();
    }

    private void UpdateKeysText(int newAmount)
    {
        keysText.text = newAmount.ToString();
    }
}
