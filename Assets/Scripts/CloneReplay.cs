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

        // SUDAH SELESAI
        if (finished) return;

        // AKHIR REPLAY
        if (currentFrame >= frames.Count)
        {
            finished = true;

            // FREEZE TOTAL
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;

                rb.bodyType = RigidbodyType2D.Static;
            }

            return;
        }

        // REPLAY POSITION
        transform.position =
            frames[currentFrame].position;

        currentFrame++;
    }
}