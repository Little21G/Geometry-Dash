using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorManager : MonoBehaviour
{
    [System.Serializable]
    public class ColorMilestone
    {
        public string colorName;
        public string hexCode;
        public float requiredScore;
        public Button primaryButton;   
        public Button secondaryButton; 
    }

    [Header("Milestone Definitions")]
    public ColorMilestone[] colorMilestones;

    [Header("Live Menu Preview")]
    [Tooltip("Drag your PlayerColorMaterial here to update the actual character preview!")]
    public Material previewMaterial; // <-- THIS talks to the shader!

    private float playerHighScore;

    void Start()
    {
        playerHighScore = PlayerPrefs.GetFloat("HighScore", 0f);
        CheckLocks();
        UpdatePreviewVisuals();
    }

    void CheckLocks()
    {
        foreach (ColorMilestone milestone in colorMilestones)
        {
            bool isUnlocked = playerHighScore >= milestone.requiredScore;

            if (milestone.primaryButton != null)
            {
                UpdateButtonUI(milestone.primaryButton, isUnlocked, milestone.requiredScore, milestone.colorName);
                milestone.primaryButton.interactable = isUnlocked;
                
                string hex = milestone.hexCode;
                milestone.primaryButton.onClick.RemoveAllListeners();
                milestone.primaryButton.onClick.AddListener(() => SelectPrimaryColor(hex));
            }

            if (milestone.secondaryButton != null)
            {
                UpdateButtonUI(milestone.secondaryButton, isUnlocked, milestone.requiredScore, milestone.colorName);
                milestone.secondaryButton.interactable = isUnlocked;
                
                string hex = milestone.hexCode;
                milestone.secondaryButton.onClick.RemoveAllListeners();
                milestone.secondaryButton.onClick.AddListener(() => SelectSecondaryColor(hex));
            }
        }
    }

    void UpdateButtonUI(Button btn, bool unlocked, float reqScore, string colName)
    {
        TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
        {
            if (unlocked) btnText.text = colName;
            else btnText.text = "🔒 " + Mathf.FloorToInt(reqScore).ToString();
        }
    }

    public void SelectPrimaryColor(string hexColor)
    {
        PlayerPrefs.SetString("PrimaryColor", hexColor);
        PlayerPrefs.Save();
        UpdatePreviewVisuals(); 
    }

    public void SelectSecondaryColor(string hexColor)
    {
        PlayerPrefs.SetString("SecondaryColor", hexColor);
        PlayerPrefs.Save();
        UpdatePreviewVisuals(); 
    }

    private void UpdatePreviewVisuals()
    {
        string primaryHex = PlayerPrefs.GetString("PrimaryColor", "#00FFFF");
        string secondaryHex = PlayerPrefs.GetString("SecondaryColor", "#FF00FF");

        // Safeguard: Ensure hex codes have the '#' symbol, otherwise Unity's color converter fails silently
        if (!primaryHex.StartsWith("#")) primaryHex = "#" + primaryHex;
        if (!secondaryHex.StartsWith("#")) secondaryHex = "#" + secondaryHex;

        if (previewMaterial != null)
        {
            if (ColorUtility.TryParseHtmlString(primaryHex, out Color primaryColor))
            {
                previewMaterial.SetColor("_PrimaryColor", primaryColor);
            }

            if (ColorUtility.TryParseHtmlString(secondaryHex, out Color secondaryColor))
            {
                previewMaterial.SetColor("_SecondaryColor", secondaryColor);
            }
        }
    }
}