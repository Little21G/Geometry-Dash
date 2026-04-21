using System.Numerics;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform player;
    public float parallaxFactor = 0.3f;

    private float startx;

    void Start() => startx = transform.position.x;

    void LateUpdate()
    {
        transform.position = new UnityEngine.Vector3(
                startx + player.position.x * parallaxFactor,
                transform.position.y,
                transform.position.z
        );
    }
}
