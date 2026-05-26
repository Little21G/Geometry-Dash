using UnityEngine;
using UnityEngine.UI; // Required for handling UI Sliders

public class VolumeSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource levelMusic;

    [Header("Save Key")]
    // This is the unique "folder name" inside the computer's registry where the value is hidden
    private string saveKey = "MusicVolume"; 
    private float defaultVolume = 0.5f; 

    void Start()
    {
        // Automatically fetch the slider if you forgot to drag it in
        if (volumeSlider == null) 
            volumeSlider = GetComponent<Slider>();

        // 1. Pull the saved volume from memory (Defaults to 0.5 if it's the first time playing)
        float savedVolume = PlayerPrefs.GetFloat(saveKey, defaultVolume);

        // 2. Set the UI slider handle and the actual music to match that saved value
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            
            // Tell the slider to trigger our function automatically whenever a player drags it
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (levelMusic != null)
        {
            levelMusic.volume = savedVolume;
        }
    }

    // 3. This runs in real-time while dragging the slider
    public void OnVolumeChanged(float value)
    {
        if (levelMusic != null)
        {
            levelMusic.volume = value;
        }

        // 4. Burn the new value into the computer's memory permanently
        PlayerPrefs.SetFloat(saveKey, value);
        PlayerPrefs.Save(); 
    }
}