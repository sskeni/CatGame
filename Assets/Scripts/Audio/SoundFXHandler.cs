using UnityEngine;

public class SoundFXHandler : MonoBehaviour
{
    private static SoundFXHandler instance;
    public static SoundFXHandler Instance { get { return instance; } }

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        CheckSingleton();
    }

    // Sets up singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float lowerPitchRange, float upperPitchRange)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = Random.Range(1f - lowerPitchRange, 1f + upperPitchRange);
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
