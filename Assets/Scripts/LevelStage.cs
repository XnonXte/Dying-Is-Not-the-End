using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;
    public string sceneName;

    [Header("Sprites")]
    public Sprite lockedSprite;
    public Sprite unplayedSprite;
    public Sprite completedSprite;

    private Image image;
    private Button button;

    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        Refresh();

        button.onClick.AddListener(PlayLevel);
    }

    void Refresh()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        bool isUnlocked = levelIndex <= unlockedLevel;
        bool isCompleted = PlayerPrefs.GetInt("LevelCompleted_" + levelIndex, 0) == 1;

        // LOCKED
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
        // UNPLAYED
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