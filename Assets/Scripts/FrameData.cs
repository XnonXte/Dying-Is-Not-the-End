using UnityEngine;

[System.Serializable]
public class FrameData
{
    public Vector3 position;
    public bool isInteracting;
    public bool isRunning;
    public bool isJump;
    public bool isFall;
    public bool flipX;

    public FrameData(Vector3 pos, bool interacting = false, bool running = false, bool jump = false, bool fall = false, bool facingLeft = false)
    {
        position = pos;
        isInteracting = interacting;
        isRunning = running;
        isJump = jump;
        isFall = fall;
        flipX = facingLeft;
    }
}