using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}