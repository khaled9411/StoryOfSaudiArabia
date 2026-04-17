using LightSide;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public LevelData[] island1Levels;
    public LevelData[] island2Levels;
    public LevelButton[] island1Buttons;
    public LevelButton[] island2Buttons;
    public GameObject island2LockedPanel;
    public UniText keysText;

    void Start()
    {
        SetupIsland1();
        SetupIsland2();
        UpdateKeysUI();
    }

    private void SetupIsland1()
    {
        for (int i = 0; i < island1Buttons.Length; i++)
        {
            if (i < island1Levels.Length)
                island1Buttons[i].Setup(island1Levels[i]);
        }
    }

    private void SetupIsland2()
    {
        bool island2Open = GameDataManager.Instance.IsIsland2Unlocked();

        island2LockedPanel.SetActive(!island2Open);

        for (int i = 0; i < island2Buttons.Length; i++)
        {
            if (i < island2Levels.Length)
                island2Buttons[i].Setup(island2Levels[i]);
        }
    }

    private void UpdateKeysUI()
    {
        int keys = PlayerDataManager.Instance.GetKeys();
        keysText.Text = $"المفاتيح: {keys} / 10";
    }
}