using UnityEngine;
using System.Collections;

public class UIIntroAnimation : MonoBehaviour
{
    [Header("Animation")]
    public Vector2 startOffset = new Vector2(0, -1000);

    public float duration = 1f;

    public float delay = 0f;

    private RectTransform rect;

    private Vector2 targetPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        // Simpan posisi asli
        targetPos = rect.anchoredPosition;

        // Mulai dari atas
        rect.anchoredPosition =
            targetPos + startOffset;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);

        float time = 0;

        Vector2 startPos = rect.anchoredPosition;

        while (time < duration)
        {
            time += Time.deltaTime;

            rect.anchoredPosition =
                Vector2.Lerp(
                    startPos,
                    targetPos,
                    time / duration
                );

            yield return null;
        }

        rect.anchoredPosition = targetPos;
    }
}