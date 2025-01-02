using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverEnPlataforma : MonoBehaviour
{
    public Rigidbody2D rb2D;
    // Referencia al Rigidbody2D del objeto para manejar su movimiento.

    public float velocidadDeMovimiento;
    // Velocidad de movimiento del objeto.

    public LayerMask capaAbajo;
    // Capa que se utiliza para detectar si hay suelo debajo del objeto.

    public LayerMask capaEnfrente;
    // Capa que se utiliza para detectar si hay un obst�culo frente al objeto.

    public float distanciaAbajo;
    // Distancia hacia abajo para detectar el suelo.

    public float distanciaEnfrente;
    // Distancia hacia adelante para detectar obst�culos.

    public Transform controladorAbajo;
    // Transform que indica la posici�n desde donde se lanza el rayo hacia abajo para detectar el suelo.

    public Transform controladorEnfrente;
    // Transform que indica la posici�n desde donde se lanza el rayo hacia adelante para detectar obst�culos.

    public bool informacionAbajo;
    // Booleano que indica si hay suelo debajo del objeto.

    public bool informacionEnfrente;
    // Booleano que indica si hay un obst�culo frente al objeto.

    private bool mirandoALaDerecha = true;
    // Indica la direcci�n hacia la que est� mirando el objeto. `true` significa hacia la derecha.

    private void Update()
    {
        // Establece la velocidad horizontal del objeto.
        rb2D.velocity = new Vector2(velocidadDeMovimiento, rb2D.velocity.y);

        // Detecta si hay un obst�culo frente al objeto.
        informacionEnfrente = Physics2D.Raycast(controladorEnfrente.position, transform.right, distanciaEnfrente, capaEnfrente);

        // Detecta si hay suelo debajo del objeto.
        informacionAbajo = Physics2D.Raycast(controladorAbajo.position, transform.up * -1, distanciaAbajo, capaAbajo);

        // Si hay un obst�culo enfrente o no hay suelo abajo, el objeto gira.
        if (informacionEnfrente || !informacionAbajo)
        {
            Girar();
        }
    }

    private void Girar()
    {
        // Cambia la direcci�n en la que est� mirando el objeto.
        mirandoALaDerecha = !mirandoALaDerecha;

        // Rota el objeto 180 grados en el eje Y para cambiar su orientaci�n.
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);

        // Invierte la direcci�n de movimiento.
        velocidadDeMovimiento *= -1;
    }

    private void OnDrawGizmos()
    {
        // Dibuja un Gizmo en el editor para mostrar la l�nea del rayo hacia abajo.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorAbajo.transform.position, controladorAbajo.transform.position + transform.up * -1 * distanciaAbajo);

        // Dibuja un Gizmo en el editor para mostrar la l�nea del rayo hacia adelante.
        Gizmos.DrawLine(controladorEnfrente.transform.position, controladorEnfrente.transform.position + transform.right * distanciaEnfrente);
    }
}
