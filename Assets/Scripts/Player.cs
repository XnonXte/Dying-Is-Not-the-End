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
    private Laser laser;

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
        if (timerStarted)
        {
            timer -= Time.deltaTime;
        }

        if (timerDisplay != null)
        {
            timerDisplay.text = "SELF DESTRUCT: " + Mathf.Max(0, (int)timer);
        }

        // === Clone Display (clone count / clone limit) ===
        if (cloneDisplay != null)
        {
            cloneDisplay.text = clones.Count + " / " + maxClone;
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

        if (timerStarted)
        {
            bool isPressingE = Keyboard.current.eKey.isPressed;
            recordedFrames.Add(new FrameData(transform.position, isPressingE));
        }
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

        if (timerStarted)
        {
            bool isPressingE = Keyboard.current.eKey.isPressed;
            recordedFrames.Add(new FrameData(transform.position, isPressingE));
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

        if (!hasSpawnedFirstClone)
        {
            timerStarted = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage") || collision.gameObject.CompareTag("Laser"))
        {
            TimeLoop();
        }


    }
}