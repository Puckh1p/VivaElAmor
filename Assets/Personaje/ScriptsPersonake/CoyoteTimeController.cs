using UnityEngine;

public class CoyoteTimeController : MonoBehaviour
{
    [SerializeField] private float coyoteTimeDuration = 0.15f;
    // Duraci�n del "Coyote Time" 

    private float coyoteTimeCounter;
    // Contador que mide cu�nto tiempo queda activo el "Coyote Time".

    public bool PuedeSaltar => coyoteTimeCounter > 0f;
    // Propiedad que devuelve `true` si el "Coyote Time" est� activo,
    // es decir, si el contador es mayor a cero. Esto se usa para permitir un salto extra
    // cuando el jugador est� cerca del suelo.

    
    /// Reinicia el "Coyote Time" al valor m�ximo, permitiendo que el jugador salte

    public void ResetCoyoteTime()
    {
        coyoteTimeCounter = coyoteTimeDuration;
    }

    
    /// Actualiza el contador del "Coyote Time" en cada frame, reduci�ndolo seg�n el tiempo transcurrido.
    /// Si el contador llega a cero, el "Coyote Time" expira y ya no permite saltar.
  
    public void UpdateCoyoteTime()
    {
        coyoteTimeCounter -= Time.deltaTime; // Resta el tiempo transcurrido desde el �ltimo frame.
    }
}
