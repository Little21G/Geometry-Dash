using UnityEngine;

public class GDBackgroundManager : MonoBehaviour
{
    [Header("Assign Your Camera")]
    public Transform cam;

    [Header("Assign Your 4 Sprites Here")]
    public SpriteRenderer bg;
    public SpriteRenderer ground;
    public SpriteRenderer lowerGround;
    public SpriteRenderer groundLine;

    [Header("Sky Speed")]
    [Tooltip("0 = fast/planted like the floor, 1 = completely stuck to the camera")]
    public float bgParallax = 0.9f;

    // Hidden variables to track where each piece started
    private float bgStart, groundStart, lowerStart, lineStart;
    
    // Hidden variables to track how wide each sprite is
    private float bgWidth, groundWidth, lowerWidth, lineWidth;

    void Start()
    {
        // If you forget to assign the camera, this finds it automatically!
        if (cam == null) cam = Camera.main.transform;

        // Record the starting positions and exact pixel widths of your sprites
        if (bg) { bgStart = bg.transform.position.x; bgWidth = bg.bounds.size.x; }
        if (ground) { groundStart = ground.transform.position.x; groundWidth = ground.bounds.size.x; }
        if (lowerGround) { lowerStart = lowerGround.transform.position.x; lowerWidth = lowerGround.bounds.size.x; }
        if (groundLine) { lineStart = groundLine.transform.position.x; lineWidth = groundLine.bounds.size.x; }
    }

    void Update()
    {
        // The background gets the parallax effect variable
        UpdateLayer(bg, ref bgStart, bgWidth, bgParallax);
        
        // The three floor pieces get a parallax of 0 (so they act like solid objects in the world)
        UpdateLayer(ground, ref groundStart, groundWidth, 0f);
        UpdateLayer(lowerGround, ref lowerStart, lowerWidth, 0f);
        UpdateLayer(groundLine, ref lineStart, lineWidth, 0f);
    }

    // This is the engine that actually moves and teleports the pieces
    void UpdateLayer(SpriteRenderer layer, ref float startPos, float width, float parallaxEffect)
    {
        if (layer == null) return; // Skip if a slot is left empty

        float temp = (cam.position.x * (1 - parallaxEffect));
        float dist = (cam.position.x * parallaxEffect);

        // Move the layer
        layer.transform.position = new Vector3(startPos + dist, layer.transform.position.y, layer.transform.position.z);

        // Teleport the layer forward or backward when the camera passes its width
        if (temp > startPos + width)
        {
            startPos += width;
        }
        else if (temp < startPos - width)
        {
            startPos -= width;
        }
    }
}