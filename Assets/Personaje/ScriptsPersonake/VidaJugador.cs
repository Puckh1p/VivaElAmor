using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para trabajar con elementos de UI como la barra de vida

// Este script gestiona la vida del jugador y cómo reacciona al recibir daño o morir.
public class VidaJugador : MonoBehaviour
{
    // Vida máxima del jugador
    public float vidaMaxima = 100f;

    // Vida actual del jugador, disminuye al recibir daño
    public float vidaActual;

    // Referencia a la barra de vida en la UI para mostrar la vida restante
    public Image barraDeVida;

    // Método Start: Se llama al iniciar el juego o al habilitar el objeto
    private void Start()
    {
        // Inicializa la vida actual con la vida máxima al empezar el juego
        vidaActual = vidaMaxima;
    }

    // Método para reducir la vida cuando el jugador recibe daño
    public void RecibirDanio(float cantidadDanio)
    {
        // Resta la cantidad de daño a la vida actual
        vidaActual -= cantidadDanio;

        // Si la vida llega a cero o menos, llama al método Morir
        if (vidaActual <= 0)
        {
            vidaActual = 0; // Asegura que la vida no sea negativa
            Morir(); // Llama al método que maneja la muerte del jugador
        }
    }

    // Método que se ejecuta cuando la vida del jugador llega a cero
    private void Morir()
    {
        // Mensaje en la consola indicando que el jugador ha muerto
        Debug.Log("El jugador ha muerto.");

        // Aquí puedes agregar cualquier lógica adicional, como reiniciar el nivel o mostrar un menú de Game Over
        // En este caso, simplemente desactivaremos el objeto del jugador
        gameObject.SetActive(false);
    }
}
