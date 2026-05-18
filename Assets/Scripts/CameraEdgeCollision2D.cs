using UnityEngine;

public class CameraEdgeCollision2D : MonoBehaviour
{
    public Camera cam;
    public float thickness = 1f;

    private BoxCollider2D top;
    private BoxCollider2D bottom;
    private BoxCollider2D left;
    private BoxCollider2D right;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        CreateWalls();
        UpdateWalls();
    }

    void CreateWalls()
    {
        top = CreateWall("Top");
        bottom = CreateWall("Bottom");
        left = CreateWall("Left");
        right = CreateWall("Right");
    }

    BoxCollider2D CreateWall(string name)
    {
        GameObject wall = new GameObject(name);
        wall.transform.parent = transform;
        return wall.AddComponent<BoxCollider2D>();
    }

    void UpdateWalls()
    {
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        Vector3 camPos = cam.transform.position;

        // Top
        top.size = new Vector2(camWidth, thickness);
        top.transform.position = new Vector3(camPos.x, camPos.y + camHeight / 2f + thickness / 2f, 0);

        // Bottom
        bottom.size = new Vector2(camWidth, thickness);
        bottom.transform.position = new Vector3(camPos.x, camPos.y - camHeight / 2f - thickness / 2f, 0);

        // Left
        left.size = new Vector2(thickness, camHeight);
        left.transform.position = new Vector3(camPos.x - camWidth / 2f - thickness / 2f, camPos.y, 0);

        // Right
        right.size = new Vector2(thickness, camHeight);
        right.transform.position = new Vector3(camPos.x + camWidth / 2f + thickness / 2f, camPos.y, 0);
    }

    void LateUpdate()
    {
        UpdateWalls(); // updates if camera moves
    }
}