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

    // M�todo Start: Se llama al inicio del juego o al habilitar el objeto
    void Start()
    {
        // Obtiene el componente Rigidbody2D del enemigo para aplicar movimiento f�sico
        rb = GetComponent<Rigidbody2D>();

        // Obtiene el componente Animator para controlar las animaciones
        anim = GetComponent<Animator>();

        // El enemigo empieza movi�ndose hacia PointB
        currentPoint = PointB.transform;

        // Activa la animaci�n de correr
        anim.SetBool("IsRunning", true);
    }

    // M�todo Update: Se llama una vez por frame
    void Update()
    {
        // Calcula la direcci�n hacia el punto objetivo actual
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

        // Si el enemigo est� cerca de PointB, cambia la direcci�n hacia PointA
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointB.transform)
        {
            flip(); // Invierte la escala en el eje X para que el enemigo mire hacia la izquierda
            currentPoint = PointA.transform; // Cambia el punto objetivo a PointA
        }
        // Si el enemigo est� cerca de PointA, cambia la direcci�n hacia PointB
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointA.transform)
        {
            flip(); // Invierte la escala en el eje X para que el enemigo mire hacia la derecha
            currentPoint = PointB.transform; // Cambia el punto objetivo a PointB
        }
    }

    // M�todo flip: Invierte la direcci�n visual del enemigo
    private void flip()
    {
        // Invierte la escala del objeto en el eje X
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // M�todo OnDrawGizmos: Dibuja elementos visuales en la escena (en el Editor de Unity)
    private void OnDrawGizmos()
    {
        // Dibuja un c�rculo alrededor de PointA para identificarlo en la escena
        Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);

        // Dibuja un c�rculo alrededor de PointB para identificarlo en la escena
        Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
    }
}
