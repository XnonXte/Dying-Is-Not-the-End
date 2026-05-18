using UnityEngine;

public class LaserDoor : MonoBehaviour
{
    [Header("Laser Visual")]
    public SpriteRenderer laser;

    [Header("Laser Collider (Damage Collider)")]
    public Collider2D laserCollider;

    [Header("Door Sprites")]
    public Sprite closedSprite;
    public Sprite openSprite;

    [Header("Trigger Sources (Optional)")]
    public Plate[] pressurePlates;
    public Button[] pedestalButtons;
    public LaserReceiver[] laserReceivers;

    [Header("Fade Settings")]
    public float fadeSpeed = 2f;

    private SpriteRenderer sr;
    private bool isDisabled = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Auto assign collider if not set manually
        if (laserCollider == null && laser != null)
            laserCollider = laser.GetComponent<Collider2D>();

        // Set initial sprite
        if (sr != null && closedSprite != null)
            sr.sprite = closedSprite;
    }

    void Update()
    {
        if (laser == null) return;

        bool platesSet = pressurePlates != null && pressurePlates.Length > 0;
        bool buttonsSet = pedestalButtons != null && pedestalButtons.Length > 0;
        bool lasersSet = laserReceivers != null && laserReceivers.Length > 0;

        bool shouldDisable = false;

        // If any triggers are assigned, laser will disable only if one trigger group is fully active
        if (platesSet || buttonsSet || lasersSet)
        {
            bool platesPressed = AllPlatesPressed(pressurePlates);
            bool buttonsPressed = AllButtonsPressed(pedestalButtons);
            bool lasersActive = AllLasersActive(laserReceivers);

            shouldDisable = platesPressed || buttonsPressed || lasersActive;
        }

        if (shouldDisable)
        {
            FadeOutLaser();

            // Change door sprite to open
            if (sr != null && openSprite != null)
                sr.sprite = openSprite;

            laserCollider.isTrigger = true;
        }
        else
        {
            FadeInLaser();

            // Change door sprite back to closed
            if (sr != null && closedSprite != null)
                sr.sprite = closedSprite;

            laserCollider.isTrigger = false;
        }
    }

    void FadeOutLaser()
    {
        // Fade alpha toward 0 (invisible)
        Color c = laser.color;
        c.a = Mathf.MoveTowards(c.a, 0f, fadeSpeed * Time.deltaTime);
        laser.color = c;

        // When fully faded out, disable collider so it no longer damages the player
        if (c.a <= 0.01f && !isDisabled)
        {
            isDisabled = true;

            if (laserCollider != null)
                laserCollider.enabled = false;
        }
    }

    void FadeInLaser()
    {
        // Fade alpha toward 1 (fully visible)
        Color c = laser.color;
        c.a = Mathf.MoveTowards(c.a, 1f, fadeSpeed * Time.deltaTime);
        laser.color = c;

        // When fully visible again, enable collider so it becomes dangerous again
        if (c.a >= 0.99f && isDisabled)
        {
            isDisabled = false;

            if (laserCollider != null)
                laserCollider.enabled = true;
        }
    }

    bool AllPlatesPressed(Plate[] plates)
    {
        if (plates == null || plates.Length == 0) return false;
        foreach (Plate p in plates)
        {
            if (p == null || !p.isPressed) return false;
        }
        return true;
    }

    bool AllButtonsPressed(Button[] buttons)
    {
        if (buttons == null || buttons.Length == 0) return false;
        foreach (Button b in buttons)
        {
            if (b == null || !b.isPressed) return false;
        }
        return true;
    }

    bool AllLasersActive(LaserReceiver[] receivers)
    {
        if (receivers == null || receivers.Length == 0) return false;
        foreach (LaserReceiver r in receivers)
        {
            if (r == null || !r.isPowered) return false;
        }
        return true;
    }
}