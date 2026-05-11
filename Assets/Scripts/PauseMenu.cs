using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseScreenUI;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public Slider musicSlider;

    [Header("Audio Reference")]
    public AudioSource gameMusic; // Drag whatever is playing your music here

    void Start()
    {
        // Make sure the menu is hidden when the game starts
        pauseScreenUI.SetActive(false);

        // Set the slider to match the music's current volume
        if (gameMusic != null)
        {
            musicSlider.value = gameMusic.volume;
            musicSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void Pause()
    {
        pauseScreenUI.SetActive(true); // Show the menu
        Time.timeScale = 0f;           // Freeze the game!

        // Get the current score from the ScoreManager
        float current = ScoreManager.instance.currentScore;
        
        // Load the High Score (defaults to 0 if we haven't set one yet)
        float high = PlayerPrefs.GetFloat("HighScore", 0f);

        // Did we beat the high score?
        if (current > high)
        {
            high = current;
            PlayerPrefs.SetFloat("HighScore", high); // Save it forever!
        }

        // Update the text on the screen
        currentScoreText.text = "Score: " + Mathf.FloorToInt(current).ToString();
        highScoreText.text = "High Score: " + Mathf.FloorToInt(high).ToString();
    }

    public void Resume()
    {
        pauseScreenUI.SetActive(false); // Hide the menu
        Time.timeScale = 1f;            // Unfreeze the game!
    }

    public void Restart()
    {
        Time.timeScale = 1f; // CRITICAL: Always unfreeze time before reloading a scene!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Quitting Game..."); // This shows in the editor so you know it worked
        Application.Quit(); // This actually closes the game when it's fully built
    }

    public void SetVolume(float volume)
    {
        if (gameMusic != null)
        {
            gameMusic.volume = volume;
        }
    }
}