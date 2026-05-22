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

        // Pause the music
        if (gameMusic != null)
        {
            gameMusic.Pause();
        }

        // Get the scores
        float current = ScoreManager.instance.currentScore;
        float high = PlayerPrefs.GetFloat("HighScore", 0f);

        // Update the menu text 
        currentScoreText.text = "Score: " + Mathf.FloorToInt(current).ToString();
        highScoreText.text = "High Score: " + Mathf.FloorToInt(high).ToString();
    }

    public void Resume()
    {
        pauseScreenUI.SetActive(false); 
        Time.timeScale = 1f;            

        // Unpause the music
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

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit(); 
    }

    public void SetVolume(float volume)
    {
        if (gameMusic != null)
        {
            gameMusic.volume = volume;
        }
    }
    
    // Optional: Add this to a button to reset your score if you get stuck testing!
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        Debug.Log("High Score Reset to 0!");
    }
}