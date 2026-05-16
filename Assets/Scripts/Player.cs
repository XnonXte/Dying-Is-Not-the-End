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
    public Transform spawnPoint;

    [Header("Clone")]
    public GameObject clonePrefab;
    public int maxClone = 2;

    private Rigidbody2D rb;

    private bool isGrounded;

    private float timer;

    private Queue<GameObject> clones =
        new Queue<GameObject>();

    private List<FrameData> recordedFrames =
        new List<FrameData>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timer = loopDuration;
    }

   void Update()
    {
        timer -= Time.deltaTime;

        // PENCET Q = LOOP LANGSUNG
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            TimeLoop();
        }

        // TIMER HABIS
        if (timer <= 0)
        {
            TimeLoop();
        }

        Move();

        // RECORD POSITION
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
        // SPAWN CLONE
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

        // LIMIT CLONE
        if (clones.Count > maxClone)
        {
            Destroy(clones.Dequeue());
        }

        // RESET PLAYER
        transform.position = spawnPoint.position;

        rb.linearVelocity = Vector2.zero;

        // RESET RECORD
        recordedFrames.Clear();

        // RESET TIMER
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