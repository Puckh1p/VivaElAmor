using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigos : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private float daño;
    [SerializeField] private float rangoAtaque;
    [SerializeField] private float cooldownAtaque = 1f; // Tiempo en segundos entre cada ataque
    private float tiempoUltimoAtaque; // Guarda el tiempo del último ataque

    private Animator animator;
    private Transform jugador;

    private void Start()
    {
        animator = GetComponent<Animator>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        tiempoUltimoAtaque = -cooldownAtaque; // Inicializa el tiempo del último ataque para permitir atacar inmediatamente al comenzar
    }

    private void Update()
    {
        // Si el jugador está dentro del rango de ataque y ha pasado el cooldown, ataca
        if (Time.time >= tiempoUltimoAtaque + cooldownAtaque && Vector2.Distance(transform.position, jugador.position) < rangoAtaque)
        {
            Atacar();
            tiempoUltimoAtaque = Time.time; // Actualiza el tiempo del último ataque
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
        
        animator.SetTrigger("EnemyAttack");


        Debug.Log("¡El enemigo está atacando!");
        if (jugador != null)
        {
            jugador.GetComponent<VidaJugador>().RecibirDanio(daño);
        }
    }
}
