using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Targeting")]
    public Transform player;

    [Header("Geometry Dash Metrics")]
    // X = 6 puts the player on the left side of the screen.
    // Y = 2 keeps the player slightly above the bottom.
    // Z = -10 is the standard 2D camera distance.
    public Vector3 offset = new Vector3(6f, 2f, -10f);
    
    [Header("Smoothing")]
    // Controls how fast the camera catches up vertically. 
    // Higher = snappier, Lower = floatier.
    public float smoothSpeedY = 5f; 

    // We use LateUpdate for cameras so it runs AFTER the player has finished moving in FixedUpdate.
    // This completely prevents screen jitter.
    void LateUpdate()
    {
        // Safety check so the game doesn't crash if the player is destroyed
        if (player == null) return;

        // 1. Strict X Lock: Instantly calculate the exact X position
        float targetX = player.position.x + offset.x;

        // 2. Smooth Y Track: Smoothly glide towards the target Y position
        float targetY = Mathf.Lerp(transform.position.y, player.position.y + offset.y, smoothSpeedY * Time.deltaTime);

        // 3. Apply the position, keeping the Z axis locked at -10
        transform.position = new Vector3(targetX, targetY, offset.z);
    }
}