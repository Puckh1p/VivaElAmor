using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuInicial : MonoBehaviour
{
    // Clip de audio que se reproducirá al iniciar el juego
    [SerializeField] private AudioClip startSound;

    // Método Jugar: Se ejecuta al presionar el botón "Jugar"
    public void Jugar()
    {
        // Carga la siguiente escena en el índice del build
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // Reproduce un sonido al iniciar el juego
        AudioManager.Instance.PlaySound(startSound);

        // Comprueba si la música está sonando, aunque no se utiliza el resultado aquí
        AudioManager.Instance.GetIsMusicPlaying();
    }

    // Método Salir: Se ejecuta al presionar el botón "Salir"
    public void Salir()
    {
        // Muestra un mensaje en la consola indicando que el jugador desea salir
        Debug.Log("Salir...");

        // Sale de la aplicación (no funciona en el editor de Unity, solo en builds)
        Application.Quit();
    }
}
