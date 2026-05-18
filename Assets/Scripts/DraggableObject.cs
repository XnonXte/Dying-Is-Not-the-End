using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Rigidbody rb;
    private bool wasKinematic;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StartDrag(Vector3 hitPoint)
    {
        isDragging = true;
        offset = transform.position - hitPoint;
        
        // Simpan state Rigidbody dan set ke kinematic saat drag
        if (rb != null)
        {
            wasKinematic = rb.isKinematic;
            rb.isKinematic = true;
        }
    }

    public void Drag(Vector3 newPosition)
    {
        if (isDragging)
        {
            transform.position = newPosition + offset;
        }
    }

    public void EndDrag()
    {
        isDragging = false;
        
        // Kembalikan state Rigidbody
        if (rb != null)
        {
            rb.isKinematic = wasKinematic;
        }
    }
}
