using UnityEngine;

public class Laser : MonoBehaviour
{
    public float maxDistance = 20f;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        Vector2 origin = transform.position;

        // arah kanan object
        Vector2 direction = transform.right;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance);

        Vector2 endPoint;

        if (hit.collider != null)
        {
            endPoint = hit.point;

            Debug.Log("Laser kena: " + hit.collider.name);
        }
        else
        {
            endPoint = origin + direction * maxDistance;
        }

        line.SetPosition(0, origin);
        line.SetPosition(1, endPoint);
    }
}