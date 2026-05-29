using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    [Header("Default Colors (Hex Codes)")]
    public string defaultPrimary = "#00FFFF"; // Cyan
    public string defaultSecondary = "#FF00FF"; // Magenta

    void Start()
    {
        LoadAndApplyColors();
    }

    public void LoadAndApplyColors()
    {
        string primaryHex = PlayerPrefs.GetString("PrimaryColor", defaultPrimary);
        string secondaryHex = PlayerPrefs.GetString("SecondaryColor", defaultSecondary);

        // Safeguard: Ensure hex codes have the '#' symbol
        if (!primaryHex.StartsWith("#")) primaryHex = "#" + primaryHex;
        if (!secondaryHex.StartsWith("#")) secondaryHex = "#" + secondaryHex;

        // Get the single SpriteRenderer attached to this GameObject
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && spriteRenderer.material != null)
        {
            if (ColorUtility.TryParseHtmlString(primaryHex, out Color primaryColor))
            {
                spriteRenderer.material.SetColor("_PrimaryColor", primaryColor);
            }

            if (ColorUtility.TryParseHtmlString(secondaryHex, out Color secondaryColor))
            {
                spriteRenderer.material.SetColor("_SecondaryColor", secondaryColor);
            }
        }
    }
}