using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    public bool isPowered { get; private set; }

    public SpriteRenderer visual;

    public Color offColor = Color.red;
    public Color onColor = Color.green;

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
        if (visual != null)
        {
            visual.color = isPowered ? onColor : offColor;
        }
    }
}