using UnityEngine;

public class GroundLineReveal : MonoBehaviour
{
    [Header("Tracking")]
    public Camera mainCamera;

    private Material mat;
    
    // We updated the shader property name to match our new logic
    private static readonly int CenterX = Shader.PropertyToID("_CenterX");

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        
        // Automatically find the camera if you forget to assign it
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null || mat == null) return;
        
        // Pass the camera's exact X position to the shader every frame
        mat.SetFloat(CenterX, mainCamera.transform.position.x);
    }
}