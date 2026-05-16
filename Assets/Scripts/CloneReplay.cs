using System.Collections.Generic;
using UnityEngine;

public class CloneReplay : MonoBehaviour
{
    public List<FrameData> frames;
    private int currentFrame;
    private Rigidbody2D rb;
    private bool finished;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        // Replay position
        transform.position =
            frames[currentFrame].position;

        // Replay interactions
        if (frames[currentFrame].isInteracting)
        {
            // Try to find and press a button at this location
            PedestalButton button = FindButtonInTrigger();
            if (button != null && !button.isPressed)
            {
                // Simulate button press by directly triggering it
                button.PressButtonDirect();
            }
        }

        currentFrame++;
    }

    private PedestalButton FindButtonInTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                PedestalButton button = collider.GetComponent<PedestalButton>();
                if (button != null)
                {
                    return button;
                }
            }
        }
        return null;
    }
}