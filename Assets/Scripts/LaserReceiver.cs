using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    public bool isPowered { get; private set; }

    [Header("Visual")]
    public SpriteRenderer visual;

    [Header("Sprites")]
    public Sprite offSprite;
    public Sprite onSprite;

    void Start()
    {
        UpdateVisual();
    }

    public void SetPowered(bool state)
    {
        isPowered = state;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (visual == null) return;

        visual.sprite = isPowered ? onSprite : offSprite;
    }
}