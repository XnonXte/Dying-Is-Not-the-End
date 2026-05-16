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

    private bool hasSpawnedFirstClone = false;     // ← New

    private MovingPlatform[] movingPlatforms;
    private PedestalButton[] pedestalButtons;

    private Queue<GameObject> clones = new Queue<GameObject>();
    private List<FrameData> recordedFrames = new List<FrameData>();

    private Transform spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingPlatforms = FindObjectsByType<MovingPlatform>(FindObjectsSortMode.None);
        pedestalButtons = FindObjectsByType<PedestalButton>(FindObjectsSortMode.None);

        timer = loopDuration;
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>();

        // Auto-start timer if this is a new loop after first clone
        if (hasSpawnedFirstClone)
        {
            timerStarted = true;
        }
    }

    void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;
        }

        if (timerDisplay != null)
        {
            timerDisplay.text = "Self Destruct: " + Mathf.Max(0, (int)timer);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame && isGrounded && timerStarted)
        {
            TimeLoop();
        }

        if (timer <= 0 && isGrounded && timerStarted)
        {
            TimeLoop();
        }

        Move();

        // Record only after timer started
        if (timerStarted)
        {
            bool isPressingE = Keyboard.current.eKey.isPressed;
            recordedFrames.Add(new FrameData(transform.position, isPressingE));
        }
    }

    void Move()
    {
        float horizontal = 0;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1;
            timerStarted = true;
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1;
            timerStarted = true;
        }

        rb.linearVelocity = new Vector2(horizontal * walkingSpeed, rb.linearVelocity.y);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            timerStarted = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void TimeLoop()
    {
        if (!timerStarted || recordedFrames.Count == 0)
        {
            ResetPlayerState();
            return;
        }

        // Spawn clone
        GameObject clone = Instantiate(clonePrefab, spawnPoint.position, Quaternion.identity);
        CloneReplay replay = clone.GetComponent<CloneReplay>();
        replay.frames = new List<FrameData>(recordedFrames);

        clones.Enqueue(clone);

        if (clones.Count > maxClone)
        {
            Destroy(clones.Dequeue());
        }

        // === Mark that first clone has been spawned ===
        hasSpawnedFirstClone = true;

        ResetPlayerState();
    }

    private void ResetPlayerState()
    {
        transform.position = spawnPoint.position;
        rb.linearVelocity = Vector2.zero;

        foreach (MovingPlatform platform in movingPlatforms)
        {
            platform.ResetToPoint1();
        }

        foreach (PedestalButton button in pedestalButtons)
        {
            button.Unpress();
        }

        recordedFrames.Clear();
        timer = loopDuration;

        // Do NOT reset timerStarted here if first clone was spawned
        // (It will be set to true in Start() for the new player)
        if (!hasSpawnedFirstClone)
        {
            timerStarted = false;
        }
        // else: timerStarted remains true (auto-start)
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            TimeLoop();
        }
    }
}