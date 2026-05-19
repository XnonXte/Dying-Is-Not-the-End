using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 7.5f;
    public float acceleration = 85f;
    public float deceleration = 110f;

    [Header("Jump")]
    public float jumpForce = 11f;

    [Header("Coyote Time + Buffer")]
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    private float coyoteTimer;
    private float jumpBufferTimer;

    [Header("Ground")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("Loop")]
    public float loopDuration = 20f;
    public TextMeshProUGUI timerDisplay;

    [Header("Clone")]
    public GameObject clonePrefab;
    public int maxClone = 2;
    public TextMeshProUGUI cloneDisplay;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float timer;
    private bool timerStarted = false;

    private bool hasSpawnedFirstClone = false;

    private Platform[] movingPlatforms;
    private Button[] pedestalButtons;

    private Queue<GameObject> clones = new Queue<GameObject>();
    private List<FrameData> recordedFrames = new List<FrameData>();

    private Transform spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        movingPlatforms = FindObjectsByType<Platform>();
        pedestalButtons = FindObjectsByType<Button>();

        timer = loopDuration;

        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>();

        if (hasSpawnedFirstClone)
        {
            timerStarted = true;
        }
    }

    void Update()
    {
        // =======================
        // TIMER LOGIC
        // =======================
        if (timerStarted)
        {
            timer -= Time.deltaTime;
        }

        if (timerDisplay != null)
        {
            timerDisplay.text = "SELF DESTRUCT: " + Mathf.Max(0, (int)timer);
        }

        if (cloneDisplay != null)
        {
            cloneDisplay.text = clones.Count + " / " + maxClone;
        }

        // =======================
        // INPUT (JUMP BUFFER)
        // =======================
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpBufferTimer = jumpBufferTime;
            timerStarted = true;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // =======================
        // RESTART / LOOP
        // =======================
        if (Keyboard.current.rKey.wasPressedThisFrame && isGrounded && timerStarted)
        {
            TimeLoop();
        }

        if (timer <= 0 && isGrounded && timerStarted)
        {
            TimeLoop();
        }

        // =======================
        // MOVEMENT
        // =======================
        Move();

        // =======================
        // RECORD CLONE DATA (ONLY ONCE PER FRAME)
        // =======================
        if (timerStarted)
        {
            bool isPressingE = Keyboard.current.eKey.isPressed;
            recordedFrames.Add(new FrameData(transform.position, isPressingE));
        }
    }

    void FixedUpdate()
    {
        // =======================
        // GROUND CHECK
        // =======================
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // =======================
        // COYOTE TIME
        // =======================
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.fixedDeltaTime;
    }

    void Move()
    {
        float horizontal = 0;

        if (Keyboard.current.aKey.isPressed)
        {
            horizontal = -1;
            timerStarted = true;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            horizontal = 1;
            timerStarted = true;
        }

        // =======================
        // ACCELERATION CURVE
        // =======================
        float targetSpeed = horizontal * maxSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float movement = speedDiff * accelRate * Time.deltaTime;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);

        // =======================
        // JUMP (BUFFER + COYOTE)
        // =======================
        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // consume both
            jumpBufferTimer = 0;
            coyoteTimer = 0;
        }
    }

    public void TimeLoop()
    {
        GameObject clone = Instantiate(clonePrefab, spawnPoint.position, Quaternion.identity);
        CloneReplay replay = clone.GetComponent<CloneReplay>();
        replay.frames = new List<FrameData>(recordedFrames);

        clones.Enqueue(clone);

        if (clones.Count > maxClone)
        {
            Destroy(clones.Dequeue());
        }

        hasSpawnedFirstClone = true;

        ResetPlayerState();
    }

    private void ResetPlayerState()
    {
        transform.position = spawnPoint.position;
        rb.linearVelocity = Vector2.zero;

        foreach (Platform platform in movingPlatforms)
        {
            platform.ResetToPoint1();
        }

        foreach (Button button in pedestalButtons)
        {
            button.Unpress();
        }

        recordedFrames.Clear();
        timer = loopDuration;

        // Timer stays started after first clone
        timerStarted = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage") || collision.gameObject.CompareTag("Laser"))
        {
            TimeLoop();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}