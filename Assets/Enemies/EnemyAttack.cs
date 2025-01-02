using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.0f;
    // Rango de ataque del enemigo. Define la distancia en la que puede detectar y atacar al jugador.

    public float attackCooldown = 2.0f;
    // Tiempo de enfriamiento entre ataques consecutivos del enemigo (en segundos).

    private float nextAttackTime = 0f;
    // Marca de tiempo que indica cuándo el enemigo puede atacar nuevamente.

    public int damage = 20;
    // Cantidad de daño que el enemigo inflige al jugador al atacar.

    private void Update()
    {
        // Comprueba si el enemigo puede atacar (el tiempo actual ha superado el próximo tiempo de ataque permitido).
        if (Time.time >= nextAttackTime)
        {
            // Detecta todos los objetos en un rango circular alrededor del enemigo.
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange);

            // Itera sobre todos los objetos detectados.
            foreach (Collider2D player in hitPlayers)
            {
                // Comprueba si el objeto detectado es el jugador.
                if (player.CompareTag("Player"))
                {
                    // Llama al método `RecibirDanio` del jugador para infligir daño.
                    player.GetComponent<ControladorJugador>().RecibirDanio(damage);

                    // Actualiza el tiempo para que el enemigo espere antes de atacar nuevamente.
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Este método se utiliza para mostrar el rango de ataque del enemigo en el editor.
        Gizmos.color = Color.red;
        // Dibuja una esfera roja en el editor que representa el rango de ataque.
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
