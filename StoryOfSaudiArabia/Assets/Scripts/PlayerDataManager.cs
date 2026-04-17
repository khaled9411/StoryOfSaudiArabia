using UnityEngine;
using System;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    public event Action<int> OnCoinsChanged;
    public event Action<int> OnKeysChanged;

    private int coins;
    private int keys;

    private const string COINS_KEY = "PlayerCoins";
    private const string KEYS_KEY = "PlayerKeys";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt(COINS_KEY, 0);
        keys = PlayerPrefs.GetInt(KEYS_KEY, 0);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveAndInvokeCoins();
    }

    public void AddKeys(int amount)
    {
        keys += amount;
        SaveAndInvokeKeys();
    }

    public bool HasEnoughCoins(int amount) => coins >= amount;
    public bool HasEnoughKeys(int amount) => keys >= amount;

    public bool SpendCoins(int amount)
    {
        if (HasEnoughCoins(amount))
        {
            coins -= amount;
            SaveAndInvokeCoins();
            return true;
        }

        Debug.Log("You don't have enough coins!");
        return false;
    }

    public bool SpendKeys(int amount)
    {
        if (HasEnoughKeys(amount))
        {
            keys -= amount;
            SaveAndInvokeKeys();
            return true;
        }

        Debug.Log("You don't have enough keys!");
        return false;
    }

    private void SaveAndInvokeCoins()
    {
        PlayerPrefs.SetInt(COINS_KEY, coins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(coins);
    }

    private void SaveAndInvokeKeys()
    {
        PlayerPrefs.SetInt(KEYS_KEY, keys);
        PlayerPrefs.Save();
        OnKeysChanged?.Invoke(keys);
    }

    public int GetCoins() => coins;
    public int GetKeys() => keys;
}