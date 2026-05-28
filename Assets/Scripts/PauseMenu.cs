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
    public AudioSource gameMusic; 

    [Header("Scene Management")]
    public string mainMenuSceneName = "MainMenuScene"; // The exact name of your main menu scene

    void Start()
    {
        pauseScreenUI.SetActive(false);

        if (gameMusic != null)
        {
            musicSlider.value = gameMusic.volume;
            musicSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void Pause()
    {
        pauseScreenUI.SetActive(true); 
        Time.timeScale = 0f;           

        if (gameMusic != null)
        {
            gameMusic.Pause();
        }

        float current = ScoreManager.instance.currentScore;
        float high = PlayerPrefs.GetFloat("HighScore", 0f);

        currentScoreText.text = "Score: " + Mathf.FloorToInt(current).ToString();
        highScoreText.text = "High Score: " + Mathf.FloorToInt(high).ToString();
    }

    public void Resume()
    {
        pauseScreenUI.SetActive(false); 
        Time.timeScale = 1f;            

        if (gameMusic != null)
        {
            gameMusic.UnPause();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- UPDATED QUIT METHOD ---
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // CRITICAL: Always unfreeze time before leaving!
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void SetVolume(float volume)
    {
        if (gameMusic != null)
        {
            gameMusic.volume = volume;
        }
    }
}