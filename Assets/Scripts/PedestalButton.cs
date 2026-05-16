using UnityEngine;
using UnityEngine.InputSystem;

public class PedestalButton : MonoBehaviour
{
    [Header("Button Settings")]
    public BoxCollider2D pressTrigger;
    public float pressedLength = 5f;
    public float moveDistance = 0.5f;
    public float moveSpeed = 2f;

    private Rigidbody2D rb;
    private float timer;
    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private bool isMovingDown;
    private bool isMovingUp;
    public bool isPressed { get; private set; }
    private bool playerInTrigger;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        pressedPosition = originalPosition - new Vector3(0, moveDistance, 0);
        timer = pressedLength;
    }

    void Update()
    {
        // Check if player is in trigger and pressed E
        if (playerInTrigger && Keyboard.current.eKey.wasPressedThisFrame && !isPressed)
        {
            PressButton();
        }

        // Handle timer
        if (isPressed)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                ReleaseButton();
            }
        }

        // Handle movement
        if (isMovingDown)
        {
            MoveDown();
        }
        else if (isMovingUp)
        {
            MoveUp();
        }
    }

    void PressButton()
    {
        isPressed = true;
        isMovingDown = true;
        timer = pressedLength;
    }

    void ReleaseButton()
    {
        isPressed = false;
        isMovingDown = false;
        isMovingUp = true;
    }

    void MoveDown()
    {
        transform.position = Vector3.MoveTowards(transform.position, pressedPosition, moveSpeed * Time.deltaTime);
        rb.position = transform.position;

        if (Vector3.Distance(transform.position, pressedPosition) < 0.01f)
        {
            isMovingDown = false;
        }
    }

    void MoveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
        rb.position = transform.position;

        if (Vector3.Distance(transform.position, originalPosition) < 0.01f)
        {
            isMovingUp = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    public void PressButtonDirect()
    {
        if (!isPressed)
        {
            PressButton();
        }
    }

    public void Unpress()
    {
        if (isPressed)
        {
            ReleaseButton();
        }
        else if (isMovingUp || isMovingDown)
        {
            // Reset to original position immediately
            transform.position = originalPosition;
            rb.position = originalPosition;
            isMovingDown = false;
            isMovingUp = false;
            isPressed = false;
        }
    }
}
