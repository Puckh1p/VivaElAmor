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
    [SerializeField] private float fuerzaSaltoLargo;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    private bool enSuelo;
    private bool salto = false;
    private bool IsJumping = false;
    private float jumpTimeCounter;
    [SerializeField] private float jumpTime;

    [Header("Dash")]
    [SerializeField] private float velocidadDash;
    [SerializeField] private float tiempoDash;
    private float gravedadInicial;
    private bool puedeHacerDash = true;
    private bool sePuedeMover = true;

    [Header("Animacion")]
    private Animator animator;

    [Header("SaltoPared")]
    [SerializeField] private Transform controladorParedIzquierda;
    [SerializeField] private Transform controladorParedDerecha;
    private bool enPared;
    private bool deslizando;
    [SerializeField] private float velocidadDeslizar;
    [SerializeField] private float fuerzaSaltoParedX;
    [SerializeField] private float fuerzaSaltoParedY;
    [SerializeField] private float tiempoSaltoPared;
    private bool saltandoDePared;

    [Header("Vida")]
    [SerializeField] private int vida = 100;
    [SerializeField] private Slider barraDeVida; // Referencia al Slider de la barra de vida

    private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 1f;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravedadInicial = rb2D.gravityScale;

        // Configurar la barra de vida
        barraDeVida.maxValue = vida;
        barraDeVida.value = vida;
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        movimientoHorizontal = inputX * velocidadDeMovimiento;

        animator.SetFloat("VelocidadX", Mathf.Abs(rb2D.velocity.x));
        animator.SetFloat("VelocidadY", rb2D.velocity.y);
        animator.SetBool("Deslizando", deslizando);

        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo)
            {
                Salto();
            }
            else if (enPared)
            {
                SaltoPared();
            }
        }

        if (Input.GetButton("Jump") && IsJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, fuerzaSaltoLargo);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            IsJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.B) && puedeHacerDash)
        {
            StartCoroutine(Dash());
        }
    }


    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        animator.SetBool("enSuelo", enSuelo);

        enPared = Physics2D.OverlapBox(controladorParedIzquierda.position, controladorParedIzquierda.GetComponent<BoxCollider2D>().size, 0f, queEsSuelo) ||
                  Physics2D.OverlapBox(controladorParedDerecha.position, controladorParedDerecha.GetComponent<BoxCollider2D>().size, 0f, queEsSuelo);
        deslizando = enPared && !enSuelo && rb2D.velocity.y < 0;

        if (sePuedeMover)
        {
            Mover(movimientoHorizontal * Time.fixedDeltaTime);
        }

        if (deslizando)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -velocidadDeslizar, float.MaxValue));
        }

        // Manejo del salto
        if (enSuelo && salto)
        {
            Salto();
        }
        else if (enPared && salto)
        {
            SaltoPared();
        }
    }

    private void Mover(float mover)
    {
        if (!saltandoDePared)
        {
            Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);
        }

        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        if (enSuelo && salto)
        {
            Salto();
        }
        else if (enPared && salto)
        {
            SaltoPared();
        }
    }

    private void Salto()
    {
        IsJumping = true;
        rb2D.velocity = new Vector2(rb2D.velocity.x, fuerzaSalto);
        jumpTimeCounter = jumpTime;
    }

    private void SaltoPared()
    {
        enPared = false;
        IsJumping = true;
        rb2D.velocity = new Vector2(rb2D.velocity.x, fuerzaSaltoParedY);
        jumpTimeCounter = jumpTime; // Reinicia el contador para el salto prolongado
        StartCoroutine(CambioSaltoPared());
    }

    private IEnumerator CambioSaltoPared()
    {
        saltandoDePared = true;
        yield return new WaitForSeconds(tiempoSaltoPared);
        saltandoDePared = false;
    }

    private IEnumerator Dash()
    {
        sePuedeMover = false;
        puedeHacerDash = false;
        rb2D.gravityScale = 0;
        rb2D.velocity = new Vector2(velocidadDash * transform.localScale.x, 0);
        animator.SetTrigger("Dash");

        yield return new WaitForSeconds(tiempoDash);

        sePuedeMover = true;
        puedeHacerDash = true;
        rb2D.gravityScale = gravedadInicial;
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
        Gizmos.color = Color.blue;
        if (controladorParedIzquierda != null)
        {
            Gizmos.DrawWireCube(controladorParedIzquierda.position, controladorParedIzquierda.GetComponent<BoxCollider2D>().size);
        }
        if (controladorParedDerecha != null)
        {
            Gizmos.DrawWireCube(controladorParedDerecha.position, controladorParedDerecha.GetComponent<BoxCollider2D>().size);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            RecibirDanio(1);
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
            Debug.LogError("GameManager instance is not set.");
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    #region ConfiguracionEditor

    // Método para ajustar el tamaño del collider del suelo y paredes desde el editor
    private void OnValidate()
    {
        // Ajusta el tamaño del collider del suelo
        if (controladorSuelo != null)
        {
            controladorSuelo.localScale = dimensionesCaja;
        }

        // Ajusta el tamaño del collider de las paredes izquierda y derecha
        if (controladorParedIzquierda != null)
        {
            controladorParedIzquierda.localScale = controladorParedIzquierda.GetComponent<BoxCollider2D>().size;
        }
        if (controladorParedDerecha != null)
        {
            controladorParedDerecha.localScale = controladorParedDerecha.GetComponent<BoxCollider2D>().size;
        }
    }

    #endregion
}
