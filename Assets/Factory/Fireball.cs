using UnityEngine;

// La clase Fireball controla el comportamiento de una bola de fuego que se mueve horizontalmente
// y puede impactar al jugador, causando daño.
public class Fireball : MonoBehaviour
{
    // Velocidad horizontal de la bola de fuego
    [SerializeField] private float velocidadHorizontal = 3f;

    // Tiempo de vida de la bola de fuego antes de destruirse automáticamente
    [SerializeField] private float tiempoDeVida = 10f;

    // Referencia al Rigidbody2D de la bola de fuego para manejar su movimiento físico
    private Rigidbody2D rb;

    // Método Start: Se ejecuta cuando la bola de fuego es creada
    private void Start()
    {
        // Obtiene el componente Rigidbody2D asociado al objeto
        rb = GetComponent<Rigidbody2D>();

        // Aplica una velocidad inicial aleatoria en el eje horizontal
        rb.velocity = new Vector2(Random.Range(-1f, 1f) * velocidadHorizontal, rb.velocity.y);

        // Programa la destrucción automática de la bola de fuego después de un tiempo definido
        Destroy(gameObject, tiempoDeVida);
    }

    // Método OnTriggerEnter2D: Se ejecuta cuando la bola de fuego colisiona con otro objeto que tiene un Collider2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Intenta obtener el componente ControladorJugador del objeto con el que colisiona
        ControladorJugador jugador = collision.gameObject.GetComponent<ControladorJugador>();

        // Si el objeto es un jugador (tiene el componente ControladorJugador):
        if (jugador != null)
        {
            // Muestra un mensaje indicando que el jugador fue impactado
            Debug.Log("Jugador impactado por Fireball.");

            // Aplica daño al jugador usando el método RecibirDanio del ControladorJugador
            jugador.RecibirDanio(1); // Aplica 1 de daño

            // Destruye la bola de fuego después de impactar al jugador
            Destroy(gameObject);
        }
        else
        {
            // Si el objeto impactado no es el jugador, muestra un mensaje en la consola
            Debug.Log("La Fireball impactó con algo que no es el jugador.");
        }
    }
}
