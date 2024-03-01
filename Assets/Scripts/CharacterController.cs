using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
 
    public float velocidad;
    public float fuerzaSalto;
    private Rigidbody2D rigidBody; 
    private bool mirandoDerecha = true;

    private void Start()
    {
      
     rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       ProcesarMovimiento(); 
       ProcesarSalto();
    }

    void ProcesarSalto()
    {
      if(Input.GetKeyDown(KeyCode.Space))
      {
        rigidBody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
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
      if((mirandoDerecha = true && inputMovimiento < 0 ) || (mirandoDerecha = false && inputMovimiento > 0 ))
      {
        mirandoDerecha = !mirandoDerecha;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
      }
    }
}
