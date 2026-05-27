using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Tracking")]
    public Transform player;
    
    [Header("Positioning")]
    [Tooltip("X is how far ahead to look. Y is how high the camera sits. Z should stay around -10.")]
    public Vector3 offset = new Vector3(4f, 2f, -10f); 

    [Header("Camera Shake")]
    public float shakeMagnitude = 0.2f;
    public float shakeTime = 0.3f;
    private float currentShakeDuration = 0f;

    void LateUpdate()
    {
        if (player != null)
        {
            // 1. Calculate the perfect position (Player's X + our custom Y height)
            Vector3 targetPosition = new Vector3(player.position.x + offset.x, offset.y, offset.z);

            // 2. Add screen shake if the player just died
            if (currentShakeDuration > 0)
            {
                targetPosition += (Vector3)Random.insideUnitCircle * shakeMagnitude;
                currentShakeDuration -= Time.deltaTime;
            }

            // 3. Move the camera
            transform.position = targetPosition;
        }
    }

    // Your Movement.cs calls this exact function when you die!
    public void TriggerShake()
    {
        currentShakeDuration = shakeTime;
    }
}