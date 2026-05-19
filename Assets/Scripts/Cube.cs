using UnityEngine;

public class Cube : MonoBehaviour
{
    public Transform initialPos;

    void Start()
    {

    }

    void Update()
    {

    }

    public void ResetBoxPosition()
    {
        transform.position = initialPos.position;
    }

}
