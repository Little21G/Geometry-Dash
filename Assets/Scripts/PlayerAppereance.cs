using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    [Header("Sprite Layers")]
    public SpriteRenderer primarySpriteRenderer;
    public SpriteRenderer secondarySpriteRenderer;

    [Header("Default Colors (Hex Codes)")]
    public string defaultPrimary = "#00FFFF"; // Cyan
    public string defaultSecondary = "#FF00FF"; // Magenta

    void Start()
    {
        LoadAndApplyColors();
    }

    public void LoadAndApplyColors()
    {
        // 1. Get the saved colors from the menu (or use the defaults if none exist yet)
        string primaryHex = PlayerPrefs.GetString("PrimaryColor", defaultPrimary);
        string secondaryHex = PlayerPrefs.GetString("SecondaryColor", defaultSecondary);

        // 2. Convert the Hex text into actual Unity Colors
        Color primaryColor;
        Color secondaryColor;

        if (ColorUtility.TryParseHtmlString(primaryHex, out primaryColor))
        {
            if (primarySpriteRenderer != null) primarySpriteRenderer.color = primaryColor;
        }

        if (ColorUtility.TryParseHtmlString(secondaryHex, out secondaryColor))
        {
            if (secondarySpriteRenderer != null) secondarySpriteRenderer.color = secondaryColor;
        }
    }
}