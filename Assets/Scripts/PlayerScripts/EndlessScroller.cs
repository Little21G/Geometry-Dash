using UnityEngine;

public class EndlessScroller : MonoBehaviour
{
    [Header("Assign Main Camera")]
    public Transform cam;

    [Header("Parallax Settings")]
    [Tooltip("0 = Solid ground, 0.9 = Slow moving sky")]
    public float parallaxSpeed = 0f;

    private Transform[] pieces;
    private float pieceWidth;
    private float totalWidth;
    private float startX;

    void Start()
    {
        // Automatically find the camera if you forget to assign it
        if (cam == null) cam = Camera.main.transform;
        
        startX = transform.position.x;

        // Gather every single block you put inside this group
        int childCount = transform.childCount;
        pieces = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            pieces[i] = transform.GetChild(i);
        }

        // Measure one piece, and calculate the total width of your entire chain
        pieceWidth = pieces[0].GetComponent<SpriteRenderer>().bounds.size.x;
        totalWidth = pieceWidth * childCount;
    }

    void Update()
    {
        // 1. Move the entire group to create the Parallax floating effect
        float dist = cam.position.x * parallaxSpeed;
        transform.position = new Vector3(startX + dist, transform.position.y, transform.position.z);

        // 2. The Chain Loop: This mathematical trick perfectly keeps the camera exactly in the middle of your blocks!
        foreach (Transform piece in pieces)
        {
            // If the camera leaves a piece too far behind on the left...
            if (cam.position.x - piece.position.x > totalWidth / 2f)
            {
                // Teleport it to the very end of the line on the right!
                piece.position += new Vector3(totalWidth, 0, 0);
            }
            // If you die or drive backwards and leave a piece on the right...
            else if (piece.position.x - cam.position.x > totalWidth / 2f)
            {
                // Teleport it back to the left!
                piece.position -= new Vector3(totalWidth, 0, 0);
            }
        }
    }
}