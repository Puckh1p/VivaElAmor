using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

// Clase DeathZone: Maneja la lógica de colisión con una zona que causa la muerte del jugador
public class DeathZone : MonoBehaviour
{
    // Clip de audio que se reproduce cuando el jugador muere
    [SerializeField] private AudioClip dieSound;

    // Método OnTriggerEnter2D: Se ejecuta cuando un objeto entra en el Collider2D de la DeathZone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisiona tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Muestra un mensaje en la consola indicando que el jugador ha colisionado con la DeathZone
            Debug.Log("El jugador ha colisionado con la zona de muerte.");

            // Invoca el método CargarEscenaDeMuerte después de un retraso de 1 segundo
            Invoke("CargarEscenaDeMuerte", 1f);
        }
    }

    // Método que carga la escena de muerte
    private void CargarEscenaDeMuerte()
    {
        // Cambia a la escena llamada "Muerte"
        SceneManager.LoadScene("Muerte");
    }

    // Método que reproduce el sonido de muerte
    private void Die()
    {
        // Usa el AudioManager para reproducir el sonido asignado en dieSound
        AudioManager.Instance.PlaySound(dieSound);
    }
}
