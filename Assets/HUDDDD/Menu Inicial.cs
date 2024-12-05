using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuInicial : MonoBehaviour
{
    // Clip de audio que se reproducir� al iniciar el juego
    [SerializeField] private AudioClip startSound;

    // M�todo Jugar: Se ejecuta al presionar el bot�n "Jugar"
    public void Jugar()
    {
        // Carga la siguiente escena en el �ndice del build
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // Reproduce un sonido al iniciar el juego
        AudioManager.Instance.PlaySound(startSound);

        // Comprueba si la m�sica est� sonando, aunque no se utiliza el resultado aqu�
        AudioManager.Instance.GetIsMusicPlaying();
    }

    // M�todo Salir: Se ejecuta al presionar el bot�n "Salir"
    public void Salir()
    {
        // Muestra un mensaje en la consola indicando que el jugador desea salir
        Debug.Log("Salir...");

        // Sale de la aplicaci�n (no funciona en el editor de Unity, solo en builds)
        Application.Quit();
    }
}
