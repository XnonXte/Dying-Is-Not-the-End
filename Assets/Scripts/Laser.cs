using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Laser")]
    public float maxDistance = 20f;

    [Header("Visual")]
    public LineRenderer lineRenderer;

    [Header("Hit")]
    public float hitCooldown = 0.2f;

    private bool canHit = true;

    private LaserReceiver currentReceiver;

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right;

        // Reset receiver sebelumnya
        if (currentReceiver != null)
        {
            currentReceiver.SetPowered(false);
            currentReceiver = null;
        }

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            direction,
            maxDistance
        );

        Vector2 endPoint;

        if (hit.collider != null)
        {
            endPoint = hit.point;

            Debug.Log("Laser kena: " + hit.collider.name);

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

            LaserReceiver receiver =
                hit.collider.GetComponent<LaserReceiver>();

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

    void DrawLaser(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void ResetHit()
    {
        canHit = true;
    }
}