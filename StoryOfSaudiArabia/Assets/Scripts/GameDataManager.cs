using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    private const string LEVEL_UNLOCKED = "LevelUnlocked_";
    private const string ISLAND2_UNLOCKED = "Island2Unlocked";
    private const int KEYS_TO_UNLOCK = 10;

    public static LevelData SelectedLevel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerDataManager.Instance != null)
            PlayerDataManager.Instance.OnKeysChanged += OnAddKey;
    }

    private void OnDestroy()
    {
        if (PlayerDataManager.Instance != null)
            PlayerDataManager.Instance.OnKeysChanged -= OnAddKey;
    }

    public void OnAddKey(int total)
    {
        if (total >= KEYS_TO_UNLOCK)
            UnlockIsland2();
    }

    private void UnlockIsland2()
    {
        PlayerPrefs.SetInt(ISLAND2_UNLOCKED, 1);
    }

    public bool IsIsland2Unlocked()
    {
        return PlayerPrefs.GetInt(ISLAND2_UNLOCKED, 0) == 1;
    }

    public void UnlockNextLevel(int currentLevelIndex)
    {
        int next = currentLevelIndex + 1;
        if (next <= 20)
            PlayerPrefs.SetInt(LEVEL_UNLOCKED + next, 1);

        PlayerPrefs.Save();
    }

    public bool IsLevelUnlocked(LevelData level)
    {
        if (level.levelIndex == 1) return true;
        if (level.islandIndex == 2 && !IsIsland2Unlocked()) return false;

        return PlayerPrefs.GetInt(LEVEL_UNLOCKED + level.levelIndex, 0) == 1;
    }

    public void OnLevelComplete(int levelIndex)
    {
        UnlockNextLevel(levelIndex);
    }
}