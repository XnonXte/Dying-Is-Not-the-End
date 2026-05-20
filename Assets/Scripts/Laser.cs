using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Laser")]
    public float maxDistance = 20f;

    [Header("Visual")]
    public LineRenderer lineRenderer;

    [Header("Hit")]
    public float hitCooldown = 0.2f;

    [Header("Activation")]
    public bool autoActive = true;

    [Tooltip("If true: triggers activate the laser. If false: triggers deactivate the laser.")]
    public bool isTriggerToActivate = true;

    [Header("Fade")]
    public float fadeDuration = 0.2f;

    [Header("Triggers (Optional)")]
    public Plate[] pressurePlates;
    public Button[] pedestalButtons;
    public LaserReceiver[] laserReceivers;

    private bool canHit = true;
    private LaserReceiver currentReceiver;

    private Coroutine fadeRoutine;
    private bool isLaserOn = false;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;

        // Start invisible
        SetLaserAlpha(0f);
        lineRenderer.enabled = false;
    }

    void Update()
    {
        bool shouldShoot = autoActive;

        bool platesSet = pressurePlates != null && pressurePlates.Length > 0;
        bool buttonsSet = pedestalButtons != null && pedestalButtons.Length > 0;
        bool lasersSet = laserReceivers != null && laserReceivers.Length > 0;

        if (platesSet || buttonsSet || lasersSet)
        {
            bool platesPressed = AllPlatesPressed(pressurePlates);
            bool buttonsPressed = AllButtonsPressed(pedestalButtons);
            bool lasersActive = AllLasersActive(laserReceivers);

            bool triggersActive = platesPressed || buttonsPressed || lasersActive;

            if (isTriggerToActivate)
                shouldShoot = triggersActive;
            else
                shouldShoot = !triggersActive;
        }

        // Fade state switching
        if (shouldShoot && !isLaserOn)
        {
            isLaserOn = true;
            StartFade(true);
        }
        else if (!shouldShoot && isLaserOn)
        {
            isLaserOn = false;
            StartFade(false);
        }

        // Only shoot if laser is ON
        if (isLaserOn)
        {
            ShootLaser();
        }
    }

    void StartFade(bool fadeIn)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeLaser(fadeIn));
    }

    IEnumerator FadeLaser(bool fadeIn)
    {
        if (fadeIn)
            lineRenderer.enabled = true;

        float startAlpha = GetLaserAlpha();
        float targetAlpha = fadeIn ? 1f : 0f;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float blend = t / fadeDuration;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, blend);
            SetLaserAlpha(alpha);

            yield return null;
        }

        SetLaserAlpha(targetAlpha);

        if (!fadeIn)
        {
            lineRenderer.enabled = false;
            DisableLaser();
        }
    }

    void ShootLaser()
    {
        if (lineRenderer == null || !lineRenderer.enabled)
            return;

        Vector2 origin = transform.position;
        Vector2 direction = transform.right;

        // Reset previous receiver
        if (currentReceiver != null)
        {
            currentReceiver.SetPowered(false);
            currentReceiver = null;
        }

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance);

        Vector2 endPoint;

        if (hit.collider != null)
        {
            endPoint = hit.point;

            // PLAYER HIT
            if (hit.collider.CompareTag("Player") && canHit)
            {
                Player player = hit.collider.GetComponent<Player>();

                if (player != null)
                {
                    canHit = false;
                    player.TimeLoop();
                    Invoke(nameof(ResetHit), hitCooldown);
                }
            }

            // RECEIVER HIT
            LaserReceiver receiver = hit.collider.GetComponent<LaserReceiver>();

            if (receiver != null)
            {
                receiver.SetPowered(true);
                currentReceiver = receiver;
            }
        }
        else
        {
            endPoint = origin + direction * maxDistance;
        }

        DrawLaser(origin, endPoint);
    }

    void DisableLaser()
    {
        // turn off receiver if laser stops
        if (currentReceiver != null)
        {
            currentReceiver.SetPowered(false);
            currentReceiver = null;
        }
    }

    void DrawLaser(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void ResetHit()
    {
        canHit = true;
    }

    void SetLaserAlpha(float alpha)
    {
        Gradient gradient = lineRenderer.colorGradient;
        GradientColorKey[] colorKeys = gradient.colorKeys;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(alpha, 0f);
        alphaKeys[1] = new GradientAlphaKey(alpha, 1f);

        Gradient newGradient = new Gradient();
        newGradient.SetKeys(colorKeys, alphaKeys);

        lineRenderer.colorGradient = newGradient;
    }

    float GetLaserAlpha()
    {
        if (lineRenderer.colorGradient.alphaKeys.Length > 0)
            return lineRenderer.colorGradient.alphaKeys[0].alpha;

        return 1f;
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