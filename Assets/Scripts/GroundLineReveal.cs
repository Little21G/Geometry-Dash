using UnityEngine;

public class GroundLineReveal : MonoBehaviour
{
    public Transform player;
    public float leadOffset = 0.5f; // how far ahead of player the line tip sits

    private Material mat;
    private static readonly int RevealX = Shader.PropertyToID("_RevealX");

    void Start()
    {
        // Get the material instance so we don't affect other objects
        mat = GetComponent<SpriteRenderer>().material;
    }

    void LateUpdate()
    {
        if (player == null || mat == null) return;
        mat.SetFloat(RevealX, player.position.x + leadOffset);
    }
}