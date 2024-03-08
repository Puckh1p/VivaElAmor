using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    private Rigidbody2D rb2D;
  [Header("Movimiento")]
  private float inputX;
  private float movimientoHorizontal = 0f;
  [SerializeField] private float velocidadDeMovimiento;
  [Range(0, 0.3f)] [SerializeField] private float suavizadoDeMovimiento;
  private Vector3 velocidad = Vector3.zero;
  private bool mirandoDerecha = true;
  [Header("Salto")]
  [SerializeField] private float fuerzaSalto;
  [SerializeField] private LayerMask queEsSuelo;
  [SerializeField] private Transform controladorSuelo;
  [SerializeField] private Vector3 dimensionesCaja;
  [SerializeField] private bool enSuelo;

  private bool salto = false;

  [Header("Dash")]

  [SerializeField] private float velocidadDash;
  [SerializeField] private float tiempoDash;

  private float gravedadInicial;
  private bool puedeHacerDash = true;

  private bool sePuedeMover = true;



  [Header("Animacion")]

  private Animator animator;

  [Header("SaltoPared")]

  [SerializeField] private Transform controladorPared;
  [SerializeField] private Vector3 dimensionesCajaPared;
  private bool enPared;
  private bool deslizando;
  [SerializeField] private float velocidadDeslizar;
  [SerializeField] private float fuerzaSaltoParedX;
  [SerializeField] private float fuerzaSaltoParedY;
  [SerializeField] private float tiempoSaltoPared;
  private bool saltandoDePared;




  private void Start()
  {
    rb2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    gravedadInicial = rb2D.gravityScale;
  }

  private void Update()

  {
    inputX = Input.GetAxisRaw("Horizontal");
    movimientoHorizontal = inputX * velocidadDeMovimiento;

    animator.SetFloat("VelocidadX", Mathf.Abs((rb2D.velocity.x)));
    animator.SetFloat("VelocidadY", rb2D.velocity.y);

    animator.SetBool("Deslizando", deslizando);

    if (Input.GetButtonDown("Jump"))
    {
        salto = true;
    }

    else

    {
        deslizando = false;
    }

    if (Input.GetKeyDown(KeyCode.B) && puedeHacerDash)

    {
        //Dash();  
        StartCoroutine(Dash());
    }
    
  }

  private void FixedUpdate()
  {
    enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
    animator.SetBool("enSuelo", enSuelo);

    enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, queEsSuelo);

    if(sePuedeMover)
    {
    Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);
    }

    salto = false;

    if(deslizando)
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -velocidadDeslizar, float.MaxValue));
    }
  }

  private void Mover(float mover, bool saltar)
  {

    if(!saltandoDePared)
    {
    Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
    rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);
    }

    if(mover > 0 && !mirandoDerecha)
    {
        Girar();
    }

    else if(mover < 0 && mirandoDerecha)
    {
        Girar();
    }
    if(saltar && enSuelo && !deslizando)
    {
        Salto();
    }
    if(saltar && enPared && !deslizando)
    {
        SaltoPared();
    }
  }

  private void SaltoPared()
  {
    enPared = false;
    rb2D.velocity = new Vector2(fuerzaSaltoParedX * -inputX,fuerzaSaltoParedY);
    StartCoroutine(CambioSaltoPared());
  }

  IEnumerator CambioSaltoPared()
  {
    saltandoDePared = true;
    yield return new WaitForSeconds(tiempoSaltoPared);
    saltandoDePared = false;
  }

  private void Salto()
  {
    enSuelo = false;
    rb2D.AddForce(new Vector2(0f, fuerzaSalto));
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
    Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
  }

}