using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;
    public string sceneName;

    [Header("Sprites")]
    public Sprite lockedSprite;      // uncomplete / locked
    public Sprite unplayedSprite;    // unlocked (belum dimainkan)
    public Sprite completedSprite;   // complete (sudah selesai)

    private Image image;
    private Button button;

    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        Refresh();

        button.onClick.AddListener(PlayLevel);
    }

    void OnEnable()
    {
        // Refresh setiap kali button di-enable (kembali dari level)
        Refresh();
    }

    void Refresh()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        bool isUnlocked = levelIndex <= unlockedLevel;
        bool isCompleted = PlayerPrefs.GetInt("LevelCompleted_" + levelIndex, 0) == 1;

        // LOCKED / UNCOMPLETE
        if (!isUnlocked)
        {
            image.sprite = lockedSprite;
            button.interactable = false;
        }
        // COMPLETED
        else if (isCompleted)
        {
            image.sprite = completedSprite;
            button.interactable = true;
        }
        // UNLOCKED - NOT COMPLETED / UNPLAYED
        else
        {
            image.sprite = unplayedSprite;
            button.interactable = true;
        }
    }

    void PlayLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}