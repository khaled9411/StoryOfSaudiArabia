using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Information")]
    public int levelIndex;
    public int islandIndex;

    [Header("Map")]
    public GameObject mapPrefab;

    [Header("Question")]
    public QuestionData questionData;
}