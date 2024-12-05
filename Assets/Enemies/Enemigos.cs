using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase Enemigos: Maneja la vida, da�o y colisiones del enemigo
public class Enemigos : MonoBehaviour
{
    // Vida inicial del enemigo
    public float vida = 100f;

    // M�todo para que el enemigo reciba da�o
    public void TomarDa�o(float cantidad)
    {
        // Reduce la vida del enemigo por la cantidad de da�o recibido
        vida -= cantidad;
        Debug.Log("Enemigo recibi� da�o. Vida restante: " + vida);

        // Si la vida llega a cero o menos, llama al m�todo Morir
        if (vida <= 0)
        {
            Morir(); // El enemigo muere
        }
    }

    // M�todo que destruye al enemigo
    private void Morir()
    {
        // Destruye el GameObject al que est� asociado este script
        Destroy(gameObject);
    }

    // M�todo que detecta colisiones con otros objetos
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Verifica si el objeto con el que colision� tiene la etiqueta "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // Si el GameManager est� configurado, llama al m�todo PerderVida
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PerderVida(); // Reduce la vida del jugador
            }
            else
            {
                // Si el GameManager no est� configurado, muestra un error en la consola
                Debug.LogError("GameManager instance is not set.");
            }
        }
    }
}
