using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float velocidadHorizontal = 3f; // Velocidad horizontal
    [SerializeField] private float tiempoDeVida = 10f; // Tiempo antes de destruirse automáticamente
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Aplica una velocidad inicial aleatoria
        rb.velocity = new Vector2(Random.Range(-1f, 1f) * velocidadHorizontal, rb.velocity.y);

        // Programar la destrucción después de 10 segundos
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ControladorJugador jugador = collision.gameObject.GetComponent<ControladorJugador>();
        if (jugador != null)
        {
            Debug.Log("Jugador impactado por Fireball.");
            jugador.RecibirDanio(1); // Aplica 1 de daño al jugador
            Destroy(gameObject); // Destruye la bola de fuego después de impactar al jugador
        }
        else
        {
            Debug.Log("La Fireball impactó con algo que no es el jugador.");
        }
    }
}
