using UnityEngine;

public class TimeSpaceEffect : MonoBehaviour
{
    [Header("Camera")]
    public Camera cam;

    [Header("Shake")]
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 5f;

    [Header("Zoom")]
    public float targetSize = 3f;
    public float zoomSpeed = 2f;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    [Header("Time")]
    public float slowTimeScale = 0.3f;

    private Vector3 originalPos;

    void Start()
    {
        originalPos = cam.transform.position;

        // slow motion
        Time.timeScale = slowTimeScale;
    }

    void Update()
    {
        // CAMERA SHAKE
        Vector3 shake =
            Random.insideUnitSphere * shakeAmount;

        cam.transform.position =
            originalPos + new Vector3(shake.x, shake.y, 0);

        // CAMERA ROTATE
        cam.transform.Rotate(0, 0,
            rotationSpeed * Time.unscaledDeltaTime);

        // CAMERA ZOOM
        cam.orthographicSize =
            Mathf.Lerp(
                cam.orthographicSize,
                targetSize,
                zoomSpeed * Time.unscaledDeltaTime
            );
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}