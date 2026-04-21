using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    public Camera cam;
    public Color targetColor = new Color(0.75f, 0.22f, 0.17f);

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Movement>() !=null)
            cam.backgroundColor = targetColor;
    }
}
