using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterController : MonoBehaviour
{

    public float velocidad;
    public float fuerzaSalto;
    public float fuerzaSaltoProlongado; // Nueva variable para el salto prolongado
    private float movimientoHorizontal;
    public LayerMask capaPiso;
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private bool mirandoDerecha = true;
    private bool saltoProlongado = false; // Variable para controlar si se está realizando un salto prolongado
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        ProcesarMovimiento();
        ProcesarSalto();
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
}

