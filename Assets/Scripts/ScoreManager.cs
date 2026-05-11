using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Makes it easy to talk to from other scripts

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
        // Add 3 points per second
        currentScore += pointsPerSecond * Time.deltaTime;

        // Update the main score UI (rounds down to a clean whole number)
        mainScoreText.text = "Score: " + Mathf.FloorToInt(currentScore).ToString();
    }

    public void AddChunkBonus()
    {
        // Calculate the escalating bonus (100, then 105, 110, etc.)
        int bonusToGive = baseChunkBonus + (chunksBeaten * bonusIncreasePerChunk);
        
        // Add it to the score and increase our chunks beaten count
        currentScore += bonusToGive;
        chunksBeaten++;

        // Spawn the cool floating text slightly below the main score!
        Vector3 spawnPos = mainScoreText.transform.position - new Vector3(0, 50f, 0); // Spawns 50 pixels lower
        GameObject popup = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity, canvasTransform);
        
        // Tell the popup what number to display
        popup.GetComponent<FloatingText>().Setup(bonusToGive);
    }
}