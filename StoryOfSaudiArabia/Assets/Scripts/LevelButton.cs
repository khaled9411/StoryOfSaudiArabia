using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelNumberText;
    public Image lockIcon;
    public Button button;

    private LevelData _levelData;

    public void Setup(LevelData data)
    {
        _levelData = data;

        levelNumberText.text = data.levelIndex.ToString();

        bool unlocked = GameDataManager.Instance.IsLevelUnlocked(data);

        lockIcon.gameObject.SetActive(!unlocked);
        button.interactable = unlocked;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameDataManager.SelectedLevel = _levelData;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}