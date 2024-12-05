using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script controla el patrullaje de un enemigo entre dos puntos (PointA y PointB).
public class EnemyPatrol : MonoBehaviour
{
    // Puntos entre los cuales el enemigo se mueve
    public GameObject PointA;
    public GameObject PointB;

    // Referencia al Rigidbody2D del enemigo para aplicar movimiento
    private Rigidbody2D rb;

    // Referencia al Animator del enemigo para controlar animaciones
    private Animator anim;

    // Puntero que indica el punto actual hacia el que se dirige el enemigo
    private Transform currentPoint;

    // Velocidad de movimiento del enemigo
    public float speed;

    // Método Start: Se llama al inicio del juego o al habilitar el objeto
    void Start()
    {
        // Obtiene el componente Rigidbody2D del enemigo para aplicar movimiento físico
        rb = GetComponent<Rigidbody2D>();

        // Obtiene el componente Animator para controlar las animaciones
        anim = GetComponent<Animator>();

        // El enemigo empieza moviéndose hacia PointB
        currentPoint = PointB.transform;

        // Activa la animación de correr
        anim.SetBool("IsRunning", true);
    }

    // Método Update: Se llama una vez por frame
    void Update()
    {
        // Calcula la dirección hacia el punto objetivo actual
        Vector2 point = currentPoint.position - transform.position;

        // Si el objetivo es PointB, mueve al enemigo hacia la derecha
        if (currentPoint == PointB.transform)
        {
            rb.velocity = new Vector2(speed, 0); // Movimiento a la derecha
        }
        else
        {
            // Si el objetivo es PointA, mueve al enemigo hacia la izquierda
            rb.velocity = new Vector2(-speed, 0); // Movimiento a la izquierda
        }

        // Si el enemigo está cerca de PointB, cambia la dirección hacia PointA
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointB.transform)
        {
            flip(); // Invierte la escala en el eje X para que el enemigo mire hacia la izquierda
            currentPoint = PointA.transform; // Cambia el punto objetivo a PointA
        }
        // Si el enemigo está cerca de PointA, cambia la dirección hacia PointB
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointA.transform)
        {
            flip(); // Invierte la escala en el eje X para que el enemigo mire hacia la derecha
            currentPoint = PointB.transform; // Cambia el punto objetivo a PointB
        }
    }

    // Método flip: Invierte la dirección visual del enemigo
    private void flip()
    {
        // Invierte la escala del objeto en el eje X
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Método OnDrawGizmos: Dibuja elementos visuales en la escena (en el Editor de Unity)
    private void OnDrawGizmos()
    {
        // Dibuja un círculo alrededor de PointA para identificarlo en la escena
        Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);

        // Dibuja un círculo alrededor de PointB para identificarlo en la escena
        Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
    }
}
