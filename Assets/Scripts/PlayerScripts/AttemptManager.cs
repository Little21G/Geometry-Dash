using UnityEngine;
using TMPro; // Needed to talk to TextMeshPro UI

public class AttemptManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI attemptText;

    void Start()
    {
        // Grab the current attempts from the computer's save file. 
        // If it hasn't been created yet, default to 1.
        int currentAttempts = PlayerPrefs.GetInt("LevelAttempts", 1);
        
        // Update the screen text
        if (attemptText != null)
        {
            attemptText.text = "ATTEMPT " + currentAttempts;
        }
    }

    // We make this "static" so the Spike script can call it instantly
    // without needing to find the manager object first.
    public static void RegisterDeath()
    {
        int currentAttempts = PlayerPrefs.GetInt("LevelAttempts", 1);
        
        // Add 1 to the attempts and save it back to the hard drive
        PlayerPrefs.SetInt("LevelAttempts", currentAttempts + 1);
        PlayerPrefs.Save();
    }
    
    void Update()
    {
        // A handy cheat code so you can reset your attempts while testing!
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.SetInt("LevelAttempts", 1);
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}