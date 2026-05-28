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
        public Button primaryButton;   // Link your UI buttons here
        public Button secondaryButton; // Link your UI buttons here
    }

    [Header("Milestone Definitions")]
    public ColorMilestone[] colorMilestones;

    private float playerHighScore;

    void Start()
    {
        // 1. Fetch the actual high score saved by ScoreManager
        playerHighScore = PlayerPrefs.GetFloat("HighScore", 0f);
        
        // 2. Refresh all buttons to see what is unlocked
        CheckLocks();
    }

    void CheckLocks()
    {
        foreach (ColorMilestone milestone in colorMilestones)
        {
            bool isUnlocked = playerHighScore >= milestone.requiredScore;

            // Handle Primary Row Button
            if (milestone.primaryButton != null)
            {
                UpdateButtonUI(milestone.primaryButton, isUnlocked, milestone.requiredScore, milestone.colorName);
                
                // If unlocked, allow clicking. If locked, disable clicking!
                milestone.primaryButton.interactable = isUnlocked;
                
                // Hook up the click behavior safely
                string hex = milestone.hexCode;
                milestone.primaryButton.onClick.RemoveAllListeners();
                milestone.primaryButton.onClick.AddListener(() => SelectPrimaryColor(hex));
            }

            // Handle Secondary Row Button
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
            if (unlocked)
            {
                btnText.text = colName; // Shows "Red", "Blue", etc.
            }
            else
            {
                btnText.text = "🔒 " + Mathf.FloorToInt(reqScore).ToString(); // Shows "🔒 2500"
            }
        }
    }

    public void SelectPrimaryColor(string hexColor)
    {
        PlayerPrefs.SetString("PrimaryColor", hexColor);
        PlayerPrefs.Save();
        Debug.Log("Equipped Primary Hex: " + hexColor);
    }

    public void SelectSecondaryColor(string hexColor)
    {
        PlayerPrefs.SetString("SecondaryColor", hexColor);
        PlayerPrefs.Save();
        Debug.Log("Equipped Secondary Hex: " + hexColor);
    }
}