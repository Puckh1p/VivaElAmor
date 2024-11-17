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
        if (collision.CompareTag("Player")) // Si colisiona con el jugador
        {
            collision.GetComponent<ControladorJugador>()?.RecibirDanio(1);
            Destroy(gameObject); // Destruye la bola de fuego tras impactar al jugador
        }
    }
}
