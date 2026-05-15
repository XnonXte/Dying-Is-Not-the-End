using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float walkingSpeed = 8;
    public float jumpForce = 4;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Rigidbody2D player;
    private bool isGrounded;
    private float groundCheckRadius = 0.2f;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = 0;
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) moveInput += 1;
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed) moveInput -= 1;

        player.linearVelocity = new Vector2(moveInput * walkingSpeed, player.linearVelocityY);
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isGrounded)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        player.linearVelocity = new Vector2(player.linearVelocityX, jumpForce);

    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damange")
        {
            Die();
        }
    }
}
