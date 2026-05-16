using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float walkingSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Ground")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Loop")]
    public float loopDuration = 20f;
    public TextMeshProUGUI timerDisplay;

    [Header("Clone")]
    public GameObject clonePrefab;
    public int maxClone = 2;

    private Rigidbody2D rb;

    private bool isGrounded;

    private float timer;
    private bool timerStarted = false;

    private MovingPlatform[] movingPlatforms;
    private PedestalButton[] pedestalButtons;

    private Queue<GameObject> clones =
        new Queue<GameObject>();

    private List<FrameData> recordedFrames =
        new List<FrameData>();
    private Transform spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingPlatforms = FindObjectsByType<MovingPlatform>();
        pedestalButtons = FindObjectsByType<PedestalButton>();
        timer = loopDuration;
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>();
    }

    void Update()
    {
        // Only count down timer if it has started
        if (timerStarted)
        {
            timer -= Time.deltaTime;
        }

        // Update timer display
        if (timerDisplay != null)
        {
            timerDisplay.text = "Self Destruct: " + Mathf.Max(0, (int)timer);
        }

        // Press R = Respawn if grounded and movement was made
        if (Keyboard.current.rKey.wasPressedThisFrame && isGrounded && timerStarted)
        {
            TimeLoop();
        }

        // Timer ends and isGroundee
        if (timer <= 0 && isGrounded && timerStarted)
        {
            TimeLoop();
        }

        Move();

        // Record position and interaction
        bool isPressingE = Keyboard.current.eKey.isPressed;
        recordedFrames.Add(
            new FrameData(transform.position, isPressingE)
        );
    }

    void Move()
    {
        float horizontal = 0;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1;
            timerStarted = true; // Start timer on movement
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1;
            timerStarted = true; // Start timer on movement
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
            timerStarted = true; // Start timer on jump
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

        // Reset all pedestal buttons
        foreach (PedestalButton button in pedestalButtons)
        {
            button.Unpress();
        }

        // Reset recording
        recordedFrames.Clear();

        // Reset timer
        timer = loopDuration;
        timerStarted = false; // Stop timer until next movement
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            TimeLoop();
        }
    }
}