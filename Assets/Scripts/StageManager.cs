using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Header("Stage Settings")]
    public int levelIndex;
    public Collider2D winCollider;

    private bool isDone = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (winCollider == null)
            Debug.LogError("Win collider tidak di-assign di StageManager!");
    }

    void Update()
    {
        if (!isDone && winCollider != null)
        {
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
        // Cek apakah player ada di win collider
        Collider2D[] colliders = Physics2D.OverlapAreaAll(
            winCollider.bounds.min,
            winCollider.bounds.max
        );

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                CompleteStage();
                return;
            }
        }
    }

    public void CompleteStage()
    {
        if (isDone) return;

        isDone = true;

        // Save level completion
        PlayerPrefs.SetInt("LevelCompleted_" + levelIndex, 1);

        // Unlock next level
        int currentUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (levelIndex >= currentUnlocked)
        {
            PlayerPrefs.SetInt("UnlockedLevel", levelIndex + 1);
        }

        PlayerPrefs.Save();

        Debug.Log("Level " + levelIndex + " Complete!");

        // Optional: show completion UI
        OnLevelComplete();
    }

    void OnLevelComplete()
    {
        // Trigger any completion effects here
        // Contoh: play sound, show UI, pause game briefly, etc.
        Time.timeScale = 0f; // Pause game

        // Show completion UI after delay
        Invoke("LoadNextScene", 2f);
    }

    void LoadNextScene()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public bool GetIsDone()
    {
        return isDone;
    }
}
