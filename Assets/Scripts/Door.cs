using UnityEngine;

public class Door : MonoBehaviour
{
    public Rigidbody2D door;  
    public Plate pressurePlate;
    public Transform doorUp;
    public Transform doorDown;
    [SerializeField] private float speed = 30f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (door == null)
            door = GetComponent<Rigidbody2D>();
    }

    // Use FixedUpdate for physics-driven movement
    void FixedUpdate()
    {
        MoveTowardTarget();
    }

    void MoveTowardTarget()
    {
        if (door == null) return;

        // default to current position / doorDown if references are missing
        Vector2 target = door.position;

        if (pressurePlate != null && pressurePlate.isPressed)
        {
            if (doorUp != null)
                target = (Vector2)doorUp.position;
        }
        else
        {
            if (doorDown != null)
                target = (Vector2)doorDown.position;
        }

        door.MovePosition(Vector2.MoveTowards(door.position, target, speed * Time.fixedDeltaTime));
    }
}
