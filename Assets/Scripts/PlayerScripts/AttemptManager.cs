using UnityEngine;
using TMPro; // Needed for TextMeshPro

public class AttemptManager : MonoBehaviour
{
    [Header("UI References")]
    // Using TMP_Text works for both UI Canvas text AND World Space text!
    public TMP_Text attemptText; 

    // 'static' means this number floats in memory and survives scene reloads,
    // but gets destroyed the moment you close the game.
    public static int currentAttempts = 1;

    void Start()
    {
        // Update the screen text the moment the level starts/reloads
        if (attemptText != null)
        {
            attemptText.text = "ATTEMPT " + currentAttempts;
        }
    }

    public static void RegisterDeath()
    {
        // Add 1 to the temporary memory
        currentAttempts++;
    }
    
    // Call this function later when you build a Main Menu "Exit" button!
    public static void ResetAttemptsOnExit()
    {
        currentAttempts = 1;
    }
}