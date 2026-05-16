using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform Point1;
    public Transform Point2;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public bool autoMove = true;

    [Header("Trigger to move (Optional)")]
    public Plate pressurePlate;
    public Button pedestalButton;
    private Rigidbody2D platform;
    private Vector3 targetPosition;
    private bool movingToPoint2 = true;

    void Start()
    {
        platform = GetComponent<Rigidbody2D>();
        if (Point1 != null)
        {
            transform.position = Point1.position;
            targetPosition = Point2.position;
        }
    }

    void Update()
    {
        // Check if platform should be moving
        bool shouldMove = autoMove;

        if (pressurePlate != null)
        {
            shouldMove = pressurePlate.isPressed;
        }

        if (pedestalButton != null)
        {
            shouldMove = pedestalButton.isPressed;
        }

        if (shouldMove && Point1 != null && Point2 != null)
        {
            MoveTowardTarget();
            CheckAndToggleTarget();
        }
    }

    void MoveTowardTarget()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        platform.position = transform.position;
    }

    void CheckAndToggleTarget()
    {
        // Check if we've reached the target position
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget < 0.1f)
        {
            // Toggle target between Point1 and Point2
            if (movingToPoint2)
            {
                targetPosition = Point1.position;
                movingToPoint2 = false;
            }
            else
            {
                targetPosition = Point2.position;
                movingToPoint2 = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }

    public void ResetToPoint1()
    {
        if (Point1 != null)
        {
            transform.position = Point1.position;
            platform.position = Point1.position;
            targetPosition = Point2.position;
            movingToPoint2 = true;
        }
    }
}
