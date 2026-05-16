using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float walkingSpeed = 8f;
    public float jumpForce = 7f;

    [Header("Ground")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Loop")]
    public float loopDuration = 20f;

    [Header("Clone")]
    public GameObject clonePrefab;
    public int maxClone = 2;

    private Rigidbody2D rb;

    private bool isGrounded;

    private float timer;

    private MovingPlatform[] movingPlatforms;

    private Queue<GameObject> clones =
        new Queue<GameObject>();

    private List<FrameData> recordedFrames =
        new List<FrameData>();
    private Transform spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingPlatforms = FindObjectsByType<MovingPlatform>();
        timer = loopDuration;
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Press Q = TimeLoop() if isGrounded
        if (Keyboard.current.qKey.wasPressedThisFrame && isGrounded)
        {
            TimeLoop();
        }

        // Timer ends and isGrounded
        if (timer <= 0 && isGrounded)
        {
            TimeLoop();
        }

        Move();

        // Record position
        recordedFrames.Add(
            new FrameData(transform.position)
        );
    }

    void Move()
    {
        float horizontal = 0;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1;
        }

        rb.linearVelocity =
            new Vector2(
                horizontal * walkingSpeed,
                rb.linearVelocity.y
            );

        if (Keyboard.current.spaceKey.wasPressedThisFrame
            && isGrounded)
        {
            rb.linearVelocity =
                new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );
        }
    }

    void FixedUpdate()
    {
        isGrounded =
            Physics2D.OverlapCircle(
                groundCheck.position,
                0.2f,
                groundLayer
            );
    }

    void TimeLoop()
    {
        // Spawn clone
        GameObject clone =
            Instantiate(
                clonePrefab,
                spawnPoint.position,
                Quaternion.identity
            );

        CloneReplay replay =
            clone.GetComponent<CloneReplay>();

        replay.frames =
            new List<FrameData>(recordedFrames);

        clones.Enqueue(clone);

        // Limit clone
        if (clones.Count > maxClone)
        {
            Destroy(clones.Dequeue());
        }

        // Reset player
        transform.position = spawnPoint.position;

        rb.linearVelocity = Vector2.zero;

        // Reset all moving platforms
        foreach (MovingPlatform platform in movingPlatforms)
        {
            platform.ResetToPoint1();
        }

        // Reset recording
        recordedFrames.Clear();

        // Reset timer
        timer = loopDuration;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            TimeLoop();
        }
    }
}