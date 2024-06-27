using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.0f;
    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;
    public int damage = 20;

    private void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange);

            foreach (Collider2D player in hitPlayers)
            {
                if (player.CompareTag("Player"))
                {
                    player.GetComponent<ControladorJugador>().RecibirDanio(damage);
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
    