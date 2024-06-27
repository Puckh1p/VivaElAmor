using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigos : MonoBehaviour
{
    public float vida = 100f;

    public void TomarDaño(float cantidad)
    {
        vida -= cantidad;
        Debug.Log("Enemigo recibió daño. Vida restante: " + vida);
        if (vida <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PerderVida();
            }
            else
            {
                Debug.LogError("GameManager instance is not set.");
            }
        }
    }
}
