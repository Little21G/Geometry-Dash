using UnityEngine;

public class CeilingSlider : MonoBehaviour
{
    [Header("Ceiling Settings")]
    public Transform ceilingObject; // Drag your grouped ceiling here
    public float startY = 15f;      // How high it starts (hidden off-screen)
    public float targetY = 8f;      // Where it stops to form the tunnel
    public float slideSpeed = 10f;  // How fast it drops down

    private bool isSliding = false;

    void Start()
    {
        // When the chunk spawns, immediately snap the ceiling to the high, hidden position
        if (ceilingObject != null)
        {
            Vector3 startPos = ceilingObject.localPosition;
            startPos.y = startY;
            ceilingObject.localPosition = startPos;
        }
    }

    void Update()
    {
        // If the player hit the trigger, start moving the ceiling down
        if (isSliding && ceilingObject != null)
        {
            Vector3 targetPos = new Vector3(ceilingObject.localPosition.x, targetY, ceilingObject.localPosition.z);
            
            // MoveTowards smoothly slides values from current to target
            ceilingObject.localPosition = Vector3.MoveTowards(ceilingObject.localPosition, targetPos, slideSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player touches our invisible trigger, start the slide!
        if (collision.CompareTag("Player"))
        {
            isSliding = true;
        }
    }
}