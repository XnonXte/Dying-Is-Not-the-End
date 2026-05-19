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

    [Header("Triggers (Optional)")]
    public Plate[] pressurePlates;
    public Button[] pedestalButtons;
    public LaserReceiver[] laserReceivers;

    private bool canHit = true;
    private LaserReceiver currentReceiver;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
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

            shouldShoot = platesPressed || buttonsPressed || lasersActive;
        }

        if (shouldShoot)
        {
            lineRenderer.enabled = true;
            ShootLaser();
        }
        else
        {
            DisableLaser();
        }
    }

    void ShootLaser()
    {
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

            // =========================
            // PLAYER HIT
            // =========================
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

            // =========================
            // RECEIVER HIT
            // =========================
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

        if (lineRenderer != null)
            lineRenderer.enabled = false;
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