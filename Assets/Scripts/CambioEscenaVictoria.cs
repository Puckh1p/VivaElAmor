using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaVictoria : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que tocó el Collider es el jugador
        {
            SceneManager.LoadScene("Ganaste"); // Cambia a la escena "Ganaste"
        }
    }
}
