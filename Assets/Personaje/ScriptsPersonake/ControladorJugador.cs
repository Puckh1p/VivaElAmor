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
    [SerializeField] private AudioClip jumpSound, finishSound, dieSound;

    private bool enSuelo;
    private int saltosRestantes;
    [SerializeField] private int maxSaltos = 2;

    [Header("Salto Prolongado")]
    [SerializeField] private float fuerzaAdicionalSalto = 5f; // Fuerza adicional para el salto prolongado
    [SerializeField] private float duracionSaltoProlongado = 0.2f; // Duración máxima del salto prolongado
    private float tiempoSaltoProlongado; // Temporizador para controlar el salto prolongado
    private bool estaSaltando; // Indica si el jugador está realizando un salto

    [Header("Dash")]
    [SerializeField] private float velocidadDash = 40f;
    [SerializeField] private float tiempoDash = 0.2f;
    [SerializeField] private float cooldownDash = 3f;
    private bool puedeHacerDash = true;
    private bool sePuedeMover = true;

    private Animator animator;

    [Header("Vida")]
    [SerializeField] private int vida = 100;
    [SerializeField] private Slider barraDeVida;

    private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 1f;

    [SerializeField] private CoyoteTimeController coyoteTimeController;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        barraDeVida.maxValue = vida;
        barraDeVida.value = vida;

        saltosRestantes = maxSaltos;
    }

    private void Update()
    {
        // Entrada horizontal y animaciones
        inputX = Input.GetAxisRaw("Horizontal");
        movimientoHorizontal = inputX * velocidadDeMovimiento;

        animator.SetFloat("VelocidadX", Mathf.Abs(rb2D.velocity.x));
        animator.SetFloat("VelocidadY", rb2D.velocity.y);

        // Detectar inicio del salto
        if (Input.GetKeyDown(KeyCode.Space) && (saltosRestantes > 0 || coyoteTimeController.PuedeSaltar))
        {
            Salto();
            estaSaltando = true;
            tiempoSaltoProlongado = 0; // Reinicia el temporizador
        }

        // Detectar salto prolongado
        if (Input.GetKey(KeyCode.Space) && estaSaltando)
        {
            if (tiempoSaltoProlongado < duracionSaltoProlongado)
            {
                // Aplica fuerza adicional mientras se mantiene presionada la tecla
                rb2D.velocity = new Vector2(rb2D.velocity.x, fuerzaSalto + fuerzaAdicionalSalto);
                tiempoSaltoProlongado += Time.deltaTime;
            }
        }

        // Detectar si se deja de presionar la tecla o el tiempo del salto prolongado se termina
        if (Input.GetKeyUp(KeyCode.Space) || tiempoSaltoProlongado >= duracionSaltoProlongado)
        {
            estaSaltando = false; // Finaliza el salto prolongado
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.E) && puedeHacerDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        if (enSuelo)
        {
            coyoteTimeController.ResetCoyoteTime(); // Reset desde el script externo
            saltosRestantes = maxSaltos; // Reset de saltos al tocar el suelo
        }
        else
        {
            coyoteTimeController.UpdateCoyoteTime(); // Actualización del coyote time
        }

        animator.SetBool("enSuelo", enSuelo);

        if (sePuedeMover)
        {
            Mover(movimientoHorizontal * Time.fixedDeltaTime);
        }
    }

    private void Mover(float mover)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Salto()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, fuerzaSalto);
        saltosRestantes--;
        //AudioManager.Instance.PlaySound(jumpSound); // Reproduce el sonido de salto
    }

    private IEnumerator Dash()
    {
        puedeHacerDash = false;
        float gravedadOriginal = rb2D.gravityScale;
        rb2D.gravityScale = 0;
        sePuedeMover = false;
        rb2D.velocity = new Vector2(velocidadDash * (mirandoDerecha ? -1 : 1), 0f);

        yield return new WaitForSeconds(tiempoDash);

        rb2D.gravityScale = gravedadOriginal;
        sePuedeMover = true;

        yield return new WaitForSeconds(cooldownDash);
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & queEsSuelo) != 0)
        {
            enSuelo = true;
        }

        if (collision.CompareTag("Enemy") && !isInvulnerable)
        {
            RecibirDanio(10);
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
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }
}
