using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Targeting")]
    public Transform player;
    
    // We need to talk to your Movement script to know the current gamemode!
    private Movement playerMovement; 

    [Header("Geometry Dash Metrics")]
    public Vector3 offset = new Vector3(6f, 2f, -10f);
    public float smoothSpeedY = 5f; 

    [Header("Camera Deadzones")]
    public float upDeadzone = 1.5f;   // How high the player can jump before the camera pans up
    public float downDeadzone = 0.5f; // How far they can fall before the camera pans down

    // We store the "ideal" floor level the camera is trying to look at
    private float targetCameraY;

    void Start()
    {
        if (player != null)
        {
            playerMovement = player.GetComponent<Movement>();
            targetCameraY = player.position.y;
        }
    }

    void LateUpdate()
    {
        if (player == null || playerMovement == null) return;

        // 1. Strict X Lock (Always moves forward)
        float targetX = player.position.x + offset.x;

        // 2. Calculate the target Y based on the Gamemode
        if (playerMovement.CurrentGamemode == Gamemodes.Ship)
        {
            // --- SHIP MODE ---
            // Do absolutely nothing to targetCameraY! 
            // It will freeze at whatever height it was at when you hit the ship portal,
            // creating that perfectly stationary Y-axis tunnel effect.
        }
        else
        {
            // --- CUBE / OTHER MODES ---
            // Calculate how far the player is from our current camera target
            float yDifference = player.position.y - targetCameraY;

            // If they jump higher than our deadzone, push the target up
            if (yDifference > upDeadzone)
            {
                targetCameraY = player.position.y - upDeadzone;
            }
            // If they fall lower than our deadzone, push the target down
            else if (yDifference < -downDeadzone)
            {
                targetCameraY = player.position.y + downDeadzone;
            }
            
            // GD Polish: If they land on flat ground, smoothly sync back up to the floor
            if (playerMovement.OnGround())
            {
                targetCameraY = player.position.y;
            }
        }

        // 3. Smooth Y Track
        // We lerp towards our calculated targetCameraY + your offset
        float smoothedY = Mathf.Lerp(transform.position.y, targetCameraY + offset.y, smoothSpeedY * Time.deltaTime);

        // 4. Apply the position
        transform.position = new Vector3(targetX, smoothedY, offset.z);
    }
}