using UnityEngine;

// Clase AudioManager: Gestiona los efectos de sonido y la música del juego.
public class AudioManager : MonoBehaviour
{
    // Referencias a componentes AudioSource para efectos de sonido (SFX) y música
    [SerializeField] private AudioSource sfxAudioSource, musicAudioSource;

    // Instancia única del AudioManager (patrón Singleton)
    public static AudioManager Instance { get; private set; }

    // Bandera que indica si la música está sonando
    private bool isMusicPlaying;

    // Método Awake: Se ejecuta antes de Start
    private void Awake()
    {
        // Implementación del patrón Singleton: asegura que solo exista un AudioManager
        if (Instance == null)
        {
            Instance = this; // Asigna esta instancia como la única
            DontDestroyOnLoad(gameObject); // Mantén este objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruye cualquier instancia duplicada
        }
    }

    // Método para reproducir un efecto de sonido
    public void PlaySound(AudioClip clip)
    {
        // Usa el AudioSource de efectos de sonido para reproducir un clip
        sfxAudioSource.PlayOneShot(clip);
    }

    // Método para verificar si la música está sonando
    public bool GetIsMusicPlaying()
    {
        // Devuelve el estado de la música
        return isMusicPlaying;
    }
}
