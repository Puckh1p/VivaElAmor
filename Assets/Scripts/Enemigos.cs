using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigos : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private float daño;
    [SerializeField] private float rangoAtaque;

    private Animator animator;
    private Transform jugador;

    private void Start()
    {
        animator = GetComponent<Animator>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Si el jugador está dentro del rango de ataque, ataca
        if (Vector2.Distance(transform.position, jugador.position) < rangoAtaque)
        {
            Atacar();
        }
    }

    public void TomarDaño(float dañoRecibido)
    {
        vida -= dañoRecibido;
        if (vida <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        animator.SetTrigger("Muerte");
        // Aquí puedes agregar cualquier otra lógica relacionada con la muerte del enemigo
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void Atacar()
    {
        // Aquí puedes agregar la lógica para realizar el ataque al jugador
        // Por ejemplo, puedes restarle vida al jugador, reproducir una animación de ataque, etc.
        Debug.Log("¡El enemigo está atacando!");
        if (jugador != null)
        {
            jugador.GetComponent<VidaJugador>().RecibirDanio(daño);
        }
    }
}
