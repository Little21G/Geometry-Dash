using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; 

    [Header("UI References")]
    public TextMeshProUGUI mainScoreText;
    public GameObject floatingTextPrefab;
    public Transform canvasTransform;

    [Header("Score Settings")]
    public float pointsPerSecond = 3f;
    public int baseChunkBonus = 100;
    public int bonusIncreasePerChunk = 5;

    public float currentScore = 0f;
    private int chunksBeaten = 0;

    void Awake()
    {
        instance = this; 
    }

    void Update()
    {
        // 1. Add points over time
        currentScore += pointsPerSecond * Time.deltaTime;

        // 2. Update Main UI
        mainScoreText.text = "Score: " + Mathf.FloorToInt(currentScore).ToString();

        // 3. ALWAYS CHECK FOR HIGH SCORE (Runs every frame)
        float currentHigh = PlayerPrefs.GetFloat("HighScore", 0f);
        if (currentScore > currentHigh)
        {
            PlayerPrefs.SetFloat("HighScore", currentScore);
            PlayerPrefs.Save(); // Forces the game to save the new record immediately
        }
    }

    public void AddChunkBonus()
    {
        int bonusToGive = baseChunkBonus + (chunksBeaten * bonusIncreasePerChunk);
        currentScore += bonusToGive;
        chunksBeaten++;

        Vector3 spawnPos = mainScoreText.transform.position - new Vector3(0, 50f, 0); 
        GameObject popup = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity, canvasTransform);
        popup.GetComponent<FloatingText>().Setup(bonusToGive);
    }
}