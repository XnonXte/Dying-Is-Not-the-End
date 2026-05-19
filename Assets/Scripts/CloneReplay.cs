using System.Collections.Generic;
using UnityEngine;

public class CloneReplay : MonoBehaviour
{
    public List<FrameData> frames;
    private int currentFrame;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool finished;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (frames == null) return;

        // Already Finished 
        if (finished) return;

        // End of replay
        if (currentFrame >= frames.Count)
        {
            finished = true;

            // Total freeze
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            return;
        }

        FrameData frame = frames[currentFrame];

        // Replay position
        transform.position = frame.position;

        if (animator != null)
        {
            animator.SetBool("isRunning", frame.isRunning);
            animator.SetBool("isJump", frame.isJump);
            animator.SetBool("isfall", frame.isFall);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = frame.flipX;
        }

        // Replay interactions
        if (frames[currentFrame].isInteracting)
        {
            // Try to find and press a button at this location
            Button button = FindButtonInTrigger();
            if (button != null && !button.isPressed)
            {
                // Simulate button press by directly triggering it
                button.PressButtonDirect();
            }
        }

        currentFrame++;
    }

    private Button FindButtonInTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                Button button = collider.GetComponent<Button>();
                if (button != null)
                {
                    return button;
                }
            }
        }
        return null;
    }
}