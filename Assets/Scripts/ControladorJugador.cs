using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorJugador : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [Header("Movimiento")]
    private float inputX;
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [Range(0, 0.3f)][SerializeField] private float suavizadoDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja = new Vector3(0.5f, 0.1f, 0f);

    private bool enSuelo;

    [Header("Salto en Pared")]
    [SerializeField] private Transform controladorParedIzquierda;
    [SerializeField] private Transform controladorParedDerecha;
    [SerializeField] private Vector3 dimensionesCajaPared = new Vector3(0.5f, 1f, 0f);
    [SerializeField] private float fuerzaSaltoParedX = 5f;
    [SerializeField] private float fuerzaSaltoParedY = 5f;
    private bool enPared;
    private bool saltandoDePared = false;

    [Header("Dash")]
    [SerializeField] private float velocidadDash;
    [SerializeField] private float tiempoDash;
    private float gravedadInicial;
    private bool puedeHacerDash = true;
    private bool sePuedeMover = true;

    [Header("Animacion")]
    private Animator animator;

    [Header("Vida")]
    [SerializeField] private int vida = 100;
    [SerializeField] private Slider barraDeVida;

    private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 1f;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        barraDeVida.maxValue = vida;
        barraDeVida.value = vida;
    }

    private void Update()
    {
        // Movimiento
        inputX = Input.GetAxisRaw("Horizontal");
        movimientoHorizontal = inputX * velocidadDeMovimiento;

        // Animaciones
        animator.SetFloat("VelocidadX", Mathf.Abs(rb2D.velocity.x));
        animator.SetFloat("VelocidadY", rb2D.velocity.y);

        // Salto normal
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            Salto();
        }

        // Salto en pared
        if (Input.GetKeyDown(KeyCode.Space) && enPared && !enSuelo)
        {
            SaltoPared();
        }

        /* Dash
        if (Input.GetKeyDown(KeyCode.B) && puedeHacerDash)
        {
            StartCoroutine(Dash());
        }
        */
    }

    private void FixedUpdate()
    {
        // Detectar si está en el suelo
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        // Detectar si está en pared
        enPared = Physics2D.OverlapBox(controladorParedIzquierda.position, dimensionesCajaPared, 0f, queEsSuelo) ||
                  Physics2D.OverlapBox(controladorParedDerecha.position, dimensionesCajaPared, 0f, queEsSuelo);

        // Animaciones
        animator.SetBool("enSuelo", enSuelo);
        animator.SetBool("enPared", enPared);

        // Movimiento
        if (sePuedeMover)
        {
            Mover(movimientoHorizontal * Time.fixedDeltaTime);
        }
    }

    private void Mover(float mover)
    {
        if (!saltandoDePared)
        {
            Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);
        }

        if (mover < 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover > 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Salto()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, fuerzaSalto);
    }

    private void SaltoPared()
    {
        enPared = false;
        saltandoDePared = true;
        rb2D.velocity = new Vector2(fuerzaSaltoParedX * (mirandoDerecha ? -1 : 1), fuerzaSaltoParedY);
        StartCoroutine(CambioSaltoPared());
    }

    private IEnumerator CambioSaltoPared()
    {
        yield return new WaitForSeconds(0.2f);
        saltandoDePared = false;
    }

    private IEnumerator Dash()
    {
        puedeHacerDash = false;
        rb2D.velocity = new Vector2(velocidadDash * (mirandoDerecha ? 1 : -1), rb2D.velocity.y);
        yield return new WaitForSeconds(tiempoDash);
        yield return new WaitForSeconds(4f); // Tiempo de espera para volver a usar el Dash
        puedeHacerDash = true;
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        // Visualizar áreas de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(controladorParedIzquierda.position, dimensionesCajaPared);
        Gizmos.DrawWireCube(controladorParedDerecha.position, dimensionesCajaPared);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detectar si el objeto que entra al trigger es el suelo
        if (((1 << collision.gameObject.layer) & queEsSuelo) != 0)
        {
            enSuelo = true;
        }

        // Detectar colisión con enemigos
        if (collision.CompareTag("Enemy") && !isInvulnerable)
        {
            RecibirDanio(10); // Aplica 10 de daño
            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    public void RecibirDanio(int danio)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PerderVida();
        }
        else
        {
            Debug.LogError("GameManager.Instance no está configurado.");
        }
    }


    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true; // El jugador es invulnerable
        yield return new WaitForSeconds(invulnerabilityDuration); // Espera la duración configurada
        isInvulnerable = false; // El jugador puede volver a recibir daño
    }

}
