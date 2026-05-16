using System.Collections.Generic;
using UnityEngine;

public class CloneReplay : MonoBehaviour
{
    public List<FrameData> frames;
    private int currentFrame;
    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private bool finished;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
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

            col.isTrigger = false;

            return;
        }

        // Replay position
        transform.position =
            frames[currentFrame].position;

        currentFrame++;
    }
}