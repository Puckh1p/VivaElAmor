using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltoDoble : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float fuerzaSalto;
    public LayerMask queEsSuelo;
    public Transform controladorSuelo;
    public Vector3 dimensionesCaja;
    private bool enSuelo;
    private bool salto = true;

    public int saltosExtrasRestantes;
    public int saltosExtra;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        if (Input.GetButtonDown("Jump"))
        {
            salto = true;
        }
        if (enSuelo)
        {
            saltosExtrasRestantes = saltosExtra;
        }
    }

    private void FixedUpdate()
    {
        Movimiento(salto);
        salto = false;
    }

    private void Movimiento(bool salto)
    {
        if (salto)
        {
            if (enSuelo)
            {
                Salto();
            }
        }
    }

    private void Salto()
    {
        rb2D.velocity = new Vector2(0f, fuerzaSalto);
        salto = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
}
