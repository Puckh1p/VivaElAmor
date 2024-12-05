using UnityEngine;

// Clase AudioManager: Gestiona los efectos de sonido y la m�sica del juego.
public class AudioManager : MonoBehaviour
{
    // Referencias a componentes AudioSource para efectos de sonido (SFX) y m�sica
    [SerializeField] private AudioSource sfxAudioSource, musicAudioSource;

    // Instancia �nica del AudioManager (patr�n Singleton)
    public static AudioManager Instance { get; private set; }

    // Bandera que indica si la m�sica est� sonando
    private bool isMusicPlaying;

    // M�todo Awake: Se ejecuta antes de Start
    private void Awake()
    {
        // Implementaci�n del patr�n Singleton: asegura que solo exista un AudioManager
        if (Instance == null)
        {
            Instance = this; // Asigna esta instancia como la �nica
            DontDestroyOnLoad(gameObject); // Mant�n este objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruye cualquier instancia duplicada
        }
    }

    // M�todo para reproducir un efecto de sonido
    public void PlaySound(AudioClip clip)
    {
        // Usa el AudioSource de efectos de sonido para reproducir un clip
        sfxAudioSource.PlayOneShot(clip);
    }

    // M�todo para verificar si la m�sica est� sonando
    public bool GetIsMusicPlaying()
    {
        // Devuelve el estado de la m�sica
        return isMusicPlaying;
    }
}
