using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaJugador : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vidaActual;
    public Image barraDeVida;

    private void Start()
    {
        vidaActual = vidaMaxima;
        
    }

    public void RecibirDanio(float cantidadDanio)
    {
        vidaActual -= cantidadDanio;
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            Morir();
        }
        
    }

    private void Morir()
    {
        // Aquí puedes agregar cualquier lógica adicional cuando la vida del jugador llegue a cero
        Debug.Log("El jugador ha muerto.");
        // Por ejemplo, puedes reiniciar la escena, mostrar un menú de Game Over, etc.
        // En este caso, simplemente desactivaremos el GameObject del jugador
        gameObject.SetActive(false);
    }

   
}
