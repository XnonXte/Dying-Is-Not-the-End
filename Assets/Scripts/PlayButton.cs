using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public CanvasGroup mainMenuCanvas;
    public CanvasGroup selectLevelCanvas;
    public CanvasGroup creditCanvas;

    public float fadeDuration = 0.5f;
    public static bool openSelectLevelOnMainMenu = false;
    [Header("Pause")]
    public CanvasGroup pauseCanvas;
    public bool isPaused = false;

    void Awake()
    {
        if (openSelectLevelOnMainMenu)
        {
            ActivateSelectLevelCanvas();
            openSelectLevelOnMainMenu = false;
        }
    }

    void ActivateSelectLevelCanvas()
    {
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.alpha = 0;
            mainMenuCanvas.interactable = false;
            mainMenuCanvas.blocksRaycasts = false;
            mainMenuCanvas.gameObject.SetActive(false);
        }

        if (selectLevelCanvas != null)
        {
            selectLevelCanvas.gameObject.SetActive(true);
            selectLevelCanvas.alpha = 1;
            selectLevelCanvas.interactable = true;
            selectLevelCanvas.blocksRaycasts = true;
        }
    }

    // Pause controls
    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;

        Time.timeScale = 0f;

        if (pauseCanvas != null)
        {
            pauseCanvas.gameObject.SetActive(true);
            pauseCanvas.alpha = 1f;
            pauseCanvas.interactable = true;
            pauseCanvas.blocksRaycasts = true;
        }
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;

        Time.timeScale = 1f;

        if (pauseCanvas != null)
        {
            pauseCanvas.alpha = 0f;
            pauseCanvas.interactable = false;
            pauseCanvas.blocksRaycasts = false;
            pauseCanvas.gameObject.SetActive(false);
        }
    }
    public void Menu(){
        // Pastikan game tidak dalam keadaan pause
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    // Retry current level (restart scene)
    public void RetryLevel()
    {
        // Ensure game is unpaused
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void playButton()
    {
        StartCoroutine(SwitchCanvas(mainMenuCanvas, selectLevelCanvas));
    }

    

    public void credit()
    {
        StartCoroutine(SwitchCanvas(mainMenuCanvas, creditCanvas));
    }

    public void back()
    {
        if (selectLevelCanvas.alpha == 1)
        {
            StartCoroutine(SwitchCanvas(selectLevelCanvas, mainMenuCanvas));
        }

        if (creditCanvas.alpha == 1)
        {
            StartCoroutine(SwitchCanvas(creditCanvas, mainMenuCanvas));
        }
    }

    IEnumerator SwitchCanvas(CanvasGroup from, CanvasGroup to)
    {
        // Fade Out
        float time = 0;

        while (time < fadeDuration)
        {
            from.alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        from.alpha = 0;
        from.interactable = false;
        from.blocksRaycasts = false;

        // Fade In
        to.gameObject.SetActive(true);

        time = 0;

        while (time < fadeDuration)
        {
            to.alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        to.alpha = 1;
        to.interactable = true;
        to.blocksRaycasts = true;
    }
}