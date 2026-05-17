using UnityEngine;

public class Plate : MonoBehaviour
{
    [Header("Pressure Plate Settings")]
    public Collider2D plateTrigger;
    public float pressDistance = 0.3f;
    public float moveSpeed = 5f;
    public string playerTag = "Player";
    public string cloneTag = "Clone";
     public string boxTag = "Box";

    private Rigidbody2D plate;
    private Vector3 initialPosition;
    private bool playerOnPlate;

    public bool isPressed { get; private set; }

    void Start()
    {
        plate = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    void Update()
    {
        // Check if player is overlapping the trigger
        playerOnPlate = CheckPlayerOnPlate();

        // Calculate target position
        Vector3 targetPosition = playerOnPlate
            ? initialPosition + Vector3.down * pressDistance
            : initialPosition;

        // Smoothly move towards target
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Update the pressed state
        isPressed = playerOnPlate;

        // Update Rigidbody position to stay synchronized
        plate.position = transform.position;
    }

    bool CheckPlayerOnPlate()
    {
        // Get all colliders overlapping the trigger
        Collider2D[] overlaps = Physics2D.OverlapAreaAll(
            plateTrigger.bounds.min,
            plateTrigger.bounds.max
        );

        // Check if any are the player
        foreach (Collider2D collider in overlaps)
        {
            if (collider.CompareTag(playerTag) || collider.CompareTag(cloneTag) || collider.CompareTag(boxTag))
            {
                return true;
            }
        }

        return false;
    }
}
