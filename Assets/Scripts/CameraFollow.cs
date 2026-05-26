using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Targeting")]
    public Transform player;
    private Movement playerMovement; 

    [Header("Geometry Dash Metrics")]
    public Vector3 offset = new Vector3(6f, 2f, -10f);
    public float smoothSpeedY = 5f; 

    [Header("Camera Deadzones")]
    public float upDeadzone = 1.5f;   
    public float downDeadzone = 0.5f; 

    private float targetCameraY;

    [Header("Screen Shake Inspector Controls")]
    // These are now fully customizable in the Unity Inspector!
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 0.4f;

    // Internal hidden variables to track the live state of the active shake
    private float shakeTimer = 0f;
    private float currentShakeMagnitude = 0f;

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

        // 1. Calculate X
        float targetX = player.position.x + offset.x;

        // 2. Calculate Y
        if (playerMovement.CurrentGamemode == Gamemodes.Ship)
        {
            // Ship mode locks the Y axis
        }
        else
        {
            float yDifference = player.position.y - targetCameraY;

            if (yDifference > upDeadzone) targetCameraY = player.position.y - upDeadzone;
            else if (yDifference < -downDeadzone) targetCameraY = player.position.y + downDeadzone;
            
            if (playerMovement.OnGround()) targetCameraY = player.position.y;
        }

        float smoothedY = Mathf.Lerp(transform.position.y, targetCameraY + offset.y, smoothSpeedY * Time.deltaTime);
        
        // Base destination of the camera
        Vector3 finalPosition = new Vector3(targetX, smoothedY, offset.z);

        // --- SCREEN SHAKE LOGIC ---
        if (shakeTimer > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * currentShakeMagnitude;
            shakeOffset.z = 0; // Lock Z axis so camera doesn't jitter forward/backward
            
            finalPosition += shakeOffset;
            shakeTimer -= Time.deltaTime;
        }

        transform.position = finalPosition;
    }

    // Now uses your serialized field settings automatically!
    public void TriggerShake()
    {
        shakeTimer = shakeDuration;
        currentShakeMagnitude = shakeMagnitude;
    }
}