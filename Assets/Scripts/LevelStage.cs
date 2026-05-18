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
    private UnityEngine.UI.Button button;

    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<UnityEngine.UI.Button>();

        Refresh();
        button.onClick.AddListener(PlayLevel);
    }

    void OnEnable()
    {
        Refresh();
    }

    void Refresh()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        bool isUnlocked = levelIndex <= unlockedLevel;
        bool isCompleted = PlayerPrefs.GetInt("LevelCompleted_" + levelIndex, 0) == 1;

        if (!isUnlocked)
        {
            image.sprite = lockedSprite;
            button.interactable = false;
        }
        else if (isCompleted)
        {
            image.sprite = completedSprite;
            button.interactable = true;
        }
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