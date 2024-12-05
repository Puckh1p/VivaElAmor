using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase Enemigos: Maneja la vida, daño y colisiones del enemigo
public class Enemigos : MonoBehaviour
{
    // Vida inicial del enemigo
    public float vida = 100f;

    // Método para que el enemigo reciba daño
    public void TomarDaño(float cantidad)
    {
        // Reduce la vida del enemigo por la cantidad de daño recibido
        vida -= cantidad;
        Debug.Log("Enemigo recibió daño. Vida restante: " + vida);

        // Si la vida llega a cero o menos, llama al método Morir
        if (vida <= 0)
        {
            Morir(); // El enemigo muere
        }
    }

    // Método que destruye al enemigo
    private void Morir()
    {
        // Destruye el GameObject al que está asociado este script
        Destroy(gameObject);
    }

    // Método que detecta colisiones con otros objetos
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Verifica si el objeto con el que colisionó tiene la etiqueta "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // Si el GameManager está configurado, llama al método PerderVida
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PerderVida(); // Reduce la vida del jugador
            }
            else
            {
                // Si el GameManager no está configurado, muestra un error en la consola
                Debug.LogError("GameManager instance is not set.");
            }
        }
    }
}
