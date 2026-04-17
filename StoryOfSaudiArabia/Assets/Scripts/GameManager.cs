using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnWinEvent;
    public event Action OnLoseEvent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TriggerWin()
    {
        OnWinEvent?.Invoke();
        Debug.Log("Player Won!");

        LevelData current = GameDataManager.SelectedLevel;

        if (current != null)
            GameDataManager.Instance.OnLevelComplete(current.levelIndex);

        StartCoroutine(TriggerWinWithDelay());
    }

    IEnumerator TriggerWinWithDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LevelSelection");
    }


    public void TriggerDeath()
    {
        OnLoseEvent?.Invoke();
        Debug.Log("Player Died!");
        Invoke(nameof(ReloadScene), 2f);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}