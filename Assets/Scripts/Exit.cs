using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [Header("Level")]
    public int currentLevel;

    [Header("Scene")]
    public string sceneToLoad;
    public GameObject levelCompleteUI;
    public GameObject UIGamePlay;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Tandai level selesai
            PlayerPrefs.SetInt("LevelCompleted_" + currentLevel, 1);

            // Ambil level terakhir yang terbuka
            int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

            // Unlock level berikutnya
            if (currentLevel >= unlockedLevel)
            {
                PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            }

            // Simpan data
            PlayerPrefs.Save();

            // Tampilkan UI level complet   e
            if (levelCompleteUI != null)
            {
                UIGamePlay.SetActive(false);
                levelCompleteUI.SetActive(true);

            }
            Time.timeScale = 0f;
            // Pindah scene
            // Freeze game

        }
    }
}