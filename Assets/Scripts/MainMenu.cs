using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject statsPanel;
    public GameObject editorPanel;

    [Header("Game Scene Name")]
    public string gameSceneName = "SampleScene"; // Change this to your actual game scene name!

    void Start()
    {
        // When the menu loads, make sure only the Main Menu is visible
        ShowMainMenu();
    }

    public void PlayGame()
    {
        // This loads your actual level!
        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        
        if (statsPanel != null) statsPanel.SetActive(false);
        if (editorPanel != null) editorPanel.SetActive(false);
    }

    public void OpenStats()
    {
        mainMenuPanel.SetActive(false);
        if (statsPanel != null) statsPanel.SetActive(true);
    }

    public void OpenPlayerEditor()
    {
        mainMenuPanel.SetActive(false);
        if (editorPanel != null) editorPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}