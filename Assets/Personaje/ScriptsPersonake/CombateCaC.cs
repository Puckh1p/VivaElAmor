using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateCaC : MonoBehaviour
{
    // Referencia a un transform que determina el punto desde donde se calculará el área de golpe
    [SerializeField] private Transform controladorGolpe;

    // Radio del área de golpe, define el alcance del ataque
    [SerializeField] private float radioGolpe;

    // Cantidad de daño que inflige el golpe
    [SerializeField] private float dañoGolpe;

    // Tiempo mínimo entre ataques consecutivos
    [SerializeField] private float tiempoEntreAtaques;

    // Contador que gestiona cuándo el jugador puede volver a atacar
    private float tiempoSiguienteAtaque;

    // Máscara de capa para filtrar qué objetos se ven afectados por el golpe (ej. enemigos, jugadores)
    [SerializeField] private LayerMask jugadorLayer;

    // Referencia al componente Animator para controlar las animaciones de ataque
    private Animator animator;

    private void Start()
    {
        // Inicializa el componente Animator asociado al GameObject
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Reduce el tiempo restante hasta que se pueda atacar nuevamente
        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }

        // Detecta si el jugador presiona el botón de ataque ("Fire1") y si se puede atacar
        if (Input.GetButtonDown("Fire1") && tiempoSiguienteAtaque <= 0)
        {
            // Ejecuta el ataque
            Golpe();

            // Reinicia el temporizador para el próximo ataque
            tiempoSiguienteAtaque = tiempoEntreAtaques;
        }
    }

    private void Golpe()
    {
        // Activa la animación de ataque
        animator.SetTrigger("Golpe");

        // Detecta todos los objetos en un área circular usando Physics2D.OverlapCircleAll
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe, jugadorLayer);

        // Itera sobre cada objeto detectado
        foreach (Collider2D colisionador in objetos)
        {
            // Verifica si el objeto tiene la etiqueta "Enemigo"
            if (colisionador.CompareTag("Enemigo"))
            {
                // Loguea el nombre del enemigo golpeado
                Debug.Log("Golpeó a: " + colisionador.name);

                // Llama al método 'TomarDaño' del enemigo para aplicar daño
                colisionador.transform.GetComponent<Enemigos>().TomarDaño(dañoGolpe);
            }
            else
            {
                // Si no es un enemigo, loguea el nombre del objeto
                Debug.Log("No es un enemigo: " + colisionador.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja un área circular en la escena para visualizar el rango de ataque (solo en el editor)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
