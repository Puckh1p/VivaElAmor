using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource, musicAudioSource;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
        }
        else
        {
        
            Destroy(this);
            DontDestroyOnLoad(this);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }
}
