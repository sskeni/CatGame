using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;

    private void Start()
    {
        // Set the slider to the correct value
        float masterVolume = PlayerPrefs.GetFloat("masterVolume"); // Get the value from player prefs
        audioMixer.SetFloat("masterVolume", masterVolume);
        float unscaledVolume = Mathf.Pow(10, masterVolume / 20f);
        masterSlider.value = unscaledVolume;
    }

    // Sets the master volume
    public void SetMasterVolume(float level)
    {
        float scaledVolume = Mathf.Log10(level) * 20f;
        audioMixer.SetFloat("masterVolume", scaledVolume);
        PlayerPrefs.SetFloat("masterVolume", scaledVolume);
    }
    
    // Sets the sound fx volume
    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
    }
    
    // Sets the music volume
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }
}
