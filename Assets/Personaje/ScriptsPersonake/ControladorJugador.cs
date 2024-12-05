using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ControladorJugador : MonoBehaviour
{
    // Referencia al Rigidbody2D para manejar el movimiento f�sico
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private float inputX; // Entrada horizontal del jugador
    private float movimientoHorizontal = 0f; // Velocidad calculada del movimiento
    [SerializeField] private float velocidadDeMovimiento; // Velocidad base de movimiento
    [Range(0, 0.3f)][SerializeField] private float suavizadoDeMovimiento; // Suavizado del movimiento para hacerlo m�s fluido
    private Vector3 velocidad = Vector3.zero; // Usado para interpolar el movimiento
    private bool mirandoDerecha = true; // Estado del jugador: si mira hacia la derecha o izquierda

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto; // Fuerza del salto
    [SerializeField] private LayerMask queEsSuelo; // Capa que define qu� objetos son "suelo"
    [SerializeField] private Transform controladorSuelo; // Posici�n para detectar si el jugador est� en el suelo
    [SerializeField] private Vector3 dimensionesCaja = new Vector3(0.5f, 0.1f, 0f); // Tama�o del �rea de detecci�n de suelo
    [SerializeField] private AudioClip jumpSound, finishSound, dieSound; // Clips de sonido para acciones del jugador

    private bool enSuelo; // Indica si el jugador est� tocando el suelo

    [Header("Salto en Pared")]
    [SerializeField] private Transform controladorParedIzquierda, controladorParedDerecha; // Puntos para detectar paredes
    [SerializeField] private Vector3 dimensionesCajaPared = new Vector3(0.5f, 1f, 0f); // Tama�o del �rea de detecci�n de paredes
    [SerializeField] private float fuerzaSaltoParedX = 5f, fuerzaSaltoParedY = 5f; // Fuerza del salto en pared en ambos ejes
    private bool enPared; // Indica si el jugador est� tocando una pared
    private bool saltandoDePared = false; // Indica si el jugador est� realizando un salto en pared

    [Header("Dash")]
    [SerializeField] private float velocidadDash; // Velocidad del Dash
    [SerializeField] private float tiempoDash; // Duraci�n del Dash
    private float gravedadInicial; // Para restaurar la gravedad despu�s del Dash
    private bool puedeHacerDash = true; // Controla si el jugador puede hacer Dash
    private bool sePuedeMover = true; // Controla si el jugador puede moverse

    [Header("Animacion")]
    private Animator animator; // Controla las animaciones del jugador

    [Header("Vida")]
    [SerializeField] private int vida = 100; // Vida inicial del jugador
    [SerializeField] private Slider barraDeVida; // Referencia a la barra de vida en la UI

    private bool isInvulnerable = false; // Controla si el jugador es invulnerable
    [SerializeField] private float invulnerabilityDuration = 1f; // Duraci�n de la invulnerabilidad

    // M�todo Start: Inicializa las variables al comienzo del juego
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); // Obtiene el componente Rigidbody2D
        animator = GetComponent<Animator>(); // Obtiene el componente Animator

        // Configura la barra de vida
        barraDeVida.maxValue = vida;
        barraDeVida.value = vida;
    }

    // M�todo Update: Llamado en cada frame para manejar la entrada del jugador
    private void Update()
    {
        // Movimiento horizontal
        inputX = Input.GetAxisRaw("Horizontal");
        movimientoHorizontal = inputX * velocidadDeMovimiento;

        // Actualiza las animaciones de velocidad
        animator.SetFloat("VelocidadX", Mathf.Abs(rb2D.velocity.x));
        animator.SetFloat("VelocidadY", rb2D.velocity.y);

        // Detecta entrada de salto
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            Salto(); // Ejecuta un salto normal
        }

        // Detecta salto en pared
        if (Input.GetKeyDown(KeyCode.Space) && enPared && !enSuelo)
        {
            SaltoPared(); // Ejecuta un salto desde una pared
        }
    }

    // M�todo FixedUpdate: Se usa para f�sica y detecci�n constante
    private void FixedUpdate()
    {
        // Detecta si est� tocando el suelo
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        // Detecta si est� tocando una pared
        enPared = Physics2D.OverlapBox(controladorParedIzquierda.position, dimensionesCajaPared, 0f, queEsSuelo) ||
                  Physics2D.OverlapBox(controladorParedDerecha.position, dimensionesCajaPared, 0f, queEsSuelo);

        // Actualiza animaciones
        animator.SetBool("enSuelo", enSuelo);
        animator.SetBool("enPared", enPared);

        // Movimiento del jugador
        if (sePuedeMover)
        {
            Mover(movimientoHorizontal * Time.fixedDeltaTime);
        }
    }

    // M�todo para manejar el movimiento del jugador
    private void Mover(float mover)
    {
        if (!saltandoDePared) // Si no est� saltando de una pared
        {
            // Suaviza el movimiento del jugador
            Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);
        }

        // Verifica y ajusta la direcci�n del jugador
        if (mover < 0 && !mirandoDerecha)
        {
            Girar(); // Cambia a mirar hacia la izquierda
        }
        else if (mover > 0 && mirandoDerecha)
        {
            Girar(); // Cambia a mirar hacia la derecha
        }
    }

    // M�todo para manejar el salto normal
    private void Salto()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, fuerzaSalto); // Aplica fuerza hacia arriba
        //AudioManager.Instance.PlaySound(jumpSound); // Reproduce el sonido de salto
    }

    // M�todo para manejar el salto en pared
    private void SaltoPared()
    {
        enPared = false; // Ya no est� en pared
        saltandoDePared = true; // Inicia el salto desde la pared
        rb2D.velocity = new Vector2(fuerzaSaltoParedX * (mirandoDerecha ? -1 : 1), fuerzaSaltoParedY); // Aplica fuerza
        StartCoroutine(CambioSaltoPared()); // Espera antes de permitir otro movimiento
    }

    // Coroutine para manejar el estado de salto desde la pared
    private IEnumerator CambioSaltoPared()
    {
        yield return new WaitForSeconds(0.2f); // Espera 0.2 segundos
        saltandoDePared = false; // Permite movimiento normal
    }

    // M�todo para girar al jugador horizontalmente
    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha; // Cambia el estado
        Vector3 escala = transform.localScale;
        escala.x *= -1; // Invierte la direcci�n
        transform.localScale = escala;
    }

    // M�todo para dibujar Gizmos en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja); // Visualiza el �rea de suelo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(controladorParedIzquierda.position, dimensionesCajaPared); // Visualiza el �rea de la pared izquierda
        Gizmos.DrawWireCube(controladorParedDerecha.position, dimensionesCajaPared); // Visualiza el �rea de la pared derecha
    }

    // M�todo para manejar la colisi�n con triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si el objeto es parte del suelo
        if (((1 << collision.gameObject.layer) & queEsSuelo) != 0)
        {
            enSuelo = true;
        }

        // Detecta si colisiona con un enemigo
        if (collision.CompareTag("Enemy") && !isInvulnerable)
        {
            RecibirDanio(10); // Aplica da�o al jugador
            StartCoroutine(InvulnerabilityCoroutine()); // Inicia invulnerabilidad temporal
        }
    }

    // M�todo para recibir da�o
    public void RecibirDanio(int danio)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PerderVida(); // Reduce la vida desde el GameManager
        }
        else
        {
            Debug.LogError("GameManager.Instance no est� configurado.");
        }
    }

    // M�todo para reproducir el sonido de muerte
    private void Die()
    {
        AudioManager.Instance.PlaySound(dieSound);
    }

    // Coroutine para manejar invulnerabilidad temporal
    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true; // Activa la invulnerabilidad
        yield return new WaitForSeconds(invulnerabilityDuration); // Espera la duraci�n configurada
        isInvulnerable = false; // Desactiva la invulnerabilidad
    }
}