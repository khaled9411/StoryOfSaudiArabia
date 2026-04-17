using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Transform mapParent;

    private GameObject _currentMap;

    void Start()
    {
        LoadSelectedLevel();
    }

    public void LoadSelectedLevel()
    {
        LevelData data = GameDataManager.SelectedLevel;

        if (data == null)
        {
            Debug.LogError("No level has been selected! Make sure GameDataManager.SelectedLevel is set.");
            return;
        }

        if (_currentMap != null)
            Destroy(_currentMap);

        _currentMap = Instantiate(data.mapPrefab, mapParent);

        FinishLine finish = _currentMap.GetComponentInChildren<FinishLine>();
        if (finish != null)
            finish.questionData = data.questionData;
        else
            Debug.LogWarning("FinishLine was not found on the map!");
    }
}