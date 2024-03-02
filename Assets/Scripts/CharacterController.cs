using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterController : MonoBehaviour
{

    public float velocidad;
    public float fuerzaSalto;
    public float fuerzaSaltoProlongado; // Nueva variable para el salto prolongado
    public float velocidadDash; // Velocidad del dash
    public float duracionDash; // Duración del dash
    public LayerMask capaPiso;
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private bool mirandoDerecha = true;
    private bool saltoProlongado = false; // Variable para controlar si se está realizando un salto prolongado
    private bool canJump = false; // Variable para controlar si se puede saltar (coyote time)
    public float coyoteTime = 0.1f;
    private bool dashing = false; // Variable para controlar si se está realizando un dash
    private float dashTimeLeft; // Tiempo restante del dash

    //SALTO PARED
    private float inputX;
    public Transform controladorPared;
    public Vector3 dimensionesCajaPared;
    public float velocidadDeslizar;
    private bool enPared;
    private bool deslizando;
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal"); //Parte del salto de pared

        ProcesarMovimiento();
        ProcesarSalto();
        ProcesarDash();

          if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Space) && (EstaEnSuelo() || canJump))
        {   
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            saltoProlongado = true; // Inicia el salto prolongado
            canJump = false; // Desactiva el coyote time
        }

        //Esta parte igual es del salto de pared
    if(!EstaEnSuelo() && enPared && inputX != 0)
    {
    deslizando = true;
    }
    else
    {
    deslizando = false;
    }

    }

    private void FixedUpdate()
    {
        //Esta parte tambien
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, capaPiso);

        if (deslizando)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -velocidadDeslizar, float.MaxValue));
        }
    }

    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, capaPiso);
        return raycastHit.collider != null;
    }

    void ProcesarSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (EstaEnSuelo())
            {   
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            saltoProlongado = true; // Inicia el salto prolongado
            }
            else if (!saltoProlongado)
            {
            saltoProlongado = true; // Inicia el salto prolongado si no está activo
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) // Cuando se suelta la tecla de espacio
        {
        saltoProlongado = false; // Finaliza el salto prolongado
        }

        // Aplica fuerza adicional si la tecla de espacio sigue presionada y el personaje está en el aire
        if (saltoProlongado && rigidBody.velocity.y > 0)
        {
        rigidBody.AddForce(Vector2.up * fuerzaSaltoProlongado * Time.deltaTime);
        }
    }

    void ProcesarMovimiento()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(inputMovimiento * velocidad, rigidBody.velocity.y);
        if (!dashing)
        {
            rigidBody.velocity = new Vector2(inputMovimiento * velocidad, rigidBody.velocity.y);
        }

        GestionarOrientacion(inputMovimiento);
    }

    void GestionarOrientacion(float inputMovimiento)
    {
        if ((mirandoDerecha == true && inputMovimiento < 0) || (mirandoDerecha == false && inputMovimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            Vector3 escala = transform.localScale;
        }
    }
    
    private void OnDrawGizmos() //Parte del codigo de salto a pared
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
    }

    void ProcesarDash()
{
  if (Input.GetKeyDown(KeyCode.C) && !dashing)
{
    StartCoroutine(Dash());
}
}

     IEnumerator Dash()
    {
        dashing = true;
        rigidBody.velocity = new Vector2(mirandoDerecha ? velocidadDash : -velocidadDash, rigidBody.velocity.y);
        dashTimeLeft = duracionDash;

        while (dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.deltaTime;
            yield return null;
        }

        dashing = false;
    }
}

