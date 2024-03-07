using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float fuerzaSaltoProlongado; 
    public float velocidadDash; 
    public float duracionDash; 
    public LayerMask capaPiso;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private bool mirandoDerecha = true;
    private bool saltoProlongado = false; 
    private bool canJump = false; 
    public float coyoteTime = 0.1f;
    private bool dashing = false; 
    private float dashTimeLeft; 
    private EstadoDash estadoDash = EstadoDash.Idle;

    // SALTO PARED
    private float inputX;
    public Transform controladorPared;
    public Vector3 dimensionesCajaPared;
    public float velocidadDeslizar;
    private bool enPared;
    private bool deslizando;

    private float tiempoCooldown = 4f;

    public enum EstadoDash
    {
        Idle,
        Dashing,
        Cooldown
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");

        ProcesarMovimiento();
        ProcesarSalto();
        ProcesarDash();

        if (Input.GetKeyDown(KeyCode.Space) && (EstaEnSuelo() || canJump))
        {
            rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            saltoProlongado = true;
            canJump = false;
        }

        if (!EstaEnSuelo() && enPared && inputX != 0)
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
                saltoProlongado = true;
            }
            else if (!saltoProlongado)
            {
                saltoProlongado = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            saltoProlongado = false;
        }

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

    void ProcesarDash()
    {
        switch (estadoDash)
        {
            case EstadoDash.Idle:
                if (Input.GetKeyDown(KeyCode.C) && estadoDash != EstadoDash.Dashing)
                {
                    Debug.Log("Activando dash");
                    StartCoroutine(Dash());
                }
                break;
            case EstadoDash.Cooldown:
                // Aquí puedes manejar cualquier lógica de cooldown si es necesario
                break;
        }
    }

    IEnumerator Dash()
    {
        estadoDash = EstadoDash.Dashing;
        dashing = true;
        rigidBody.velocity = new Vector2(mirandoDerecha ? velocidadDash : -velocidadDash, rigidBody.velocity.y);
        dashTimeLeft = duracionDash;

        while (dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.deltaTime;
            yield return null;
        }

        dashing = false;
        estadoDash = EstadoDash.Cooldown;
        yield return new WaitForSeconds(tiempoCooldown); // Cooldown de 4 segundos
        estadoDash = EstadoDash.Idle;
    }
}