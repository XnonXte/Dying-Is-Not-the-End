using UnityEngine;

public class Exit : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("PLAYER EXITED");
        }
    }
}
