using UnityEngine;

public class MenuCameraLoop : MonoBehaviour
{
    public float speed = 2f;

    public float startX;
    public float endX;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x >= endX)
        {
            Vector3 pos = transform.position;
            pos.x = startX;

            transform.position = pos;
        }
    }
} 
