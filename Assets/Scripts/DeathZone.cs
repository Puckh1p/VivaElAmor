using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que colisionó es el jugador
        {
            // Aquí podrías activar efectos de sonido, animaciones de muerte, etc.

            Debug.Log("El jugador ha colisionado con la zona de muerte.");

            // Después de un breve retraso, cambia a la escena de muerte
            Invoke("CargarEscenaDeMuerte", 1f);
        }
    }

    private void CargarEscenaDeMuerte()
    {
        SceneManager.LoadScene("Muerte"); // Cambia a la escena de muerte
    }
}
