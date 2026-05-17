using UnityEngine;

public class Door : MonoBehaviour
{
    public Plate[] pressurePlates;
    public Button[] pedestalButtons;
    public LaserReceiver[] laserReceivers; // NEW: Array of laser receivers that can trigger the door
    public Transform doorUp;
    public Transform doorDown;
    private Rigidbody2D door;

    [SerializeField] private float speed = 30f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        door = GetComponent<Rigidbody2D>();
    }

    // Use FixedUpdate for physics-driven movement
    void FixedUpdate()
    {
        MoveTowardTarget();
    }

    void MoveTowardTarget()
    {
        if (door == null) return;

        // default to current position / doorDown if references are missing
        Vector2 target = door.position;
            bool platesOpen = AllPlatesPressed(pressurePlates);
            bool buttonsOpen = AllButtonsPressed(pedestalButtons);
            bool lasersOpen = AllLasersActive(laserReceivers);

            if (platesOpen || buttonsOpen || lasersOpen)
            {
                if (doorUp != null)
                    target = (Vector2)doorUp.position;
            }
            else
            {
                if (doorDown != null)
                    target = (Vector2)doorDown.position;
            }
        door.MovePosition(Vector2.MoveTowards(door.position, target, speed * Time.fixedDeltaTime));
    }

        bool AllPlatesPressed(Plate[] plates)
        {
            if (plates == null || plates.Length == 0) return false;
            foreach (Plate p in plates)
            {
                if (p == null || !p.isPressed) return false;
            }
            return true;
        }

        bool AllButtonsPressed(Button[] buttons)
        {
            if (buttons == null || buttons.Length == 0) return false;
            foreach (Button b in buttons)
            {
                if (b == null || !b.isPressed) return false;
            }
            return true;
        }
        bool AllLasersActive(LaserReceiver[] receivers)
        {
            if (receivers == null || receivers.Length == 0) return false;
            foreach (LaserReceiver r in receivers)
            {
                if (r == null || !r.isPowered) return false;
            }
            return true;
        }
}