using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [Header("Level")]
    public int currentLevel;

    [Header("Scene")]
    public string sceneToLoad;

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

            // Jika ke MainMenu, buka langsung Select Level
            if (sceneToLoad == "MainMenu")
            {
                PlayButton.openSelectLevelOnMainMenu = true;
            }

            // Pindah scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}