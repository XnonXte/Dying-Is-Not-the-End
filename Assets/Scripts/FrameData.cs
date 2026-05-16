using UnityEngine;

[System.Serializable]
public class FrameData
{
    public Vector3 position;
    public bool isInteracting;

    public FrameData(Vector3 pos, bool interacting = false)
    {
        position = pos;
        isInteracting = interacting;
    }
}