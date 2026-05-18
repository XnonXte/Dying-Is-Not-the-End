using System.Collections;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public CanvasGroup mainMenuCanvas;
    public CanvasGroup selectLevelCanvas;
    public CanvasGroup creditCanvas;

    public float fadeDuration = 0.5f;

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