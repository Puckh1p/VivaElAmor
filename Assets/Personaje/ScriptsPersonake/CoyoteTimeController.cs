using UnityEngine;

public class CoyoteTimeController : MonoBehaviour
{
    [SerializeField] private float coyoteTimeDuration = 0.15f;
    // Duración del "Coyote Time" 

    private float coyoteTimeCounter;
    // Contador que mide cuánto tiempo queda activo el "Coyote Time".

    public bool PuedeSaltar => coyoteTimeCounter > 0f;
    // Propiedad que devuelve `true` si el "Coyote Time" está activo,
    // es decir, si el contador es mayor a cero. Esto se usa para permitir un salto extra
    // cuando el jugador está cerca del suelo.

    
    /// Reinicia el "Coyote Time" al valor máximo, permitiendo que el jugador salte

    public void ResetCoyoteTime()
    {
        coyoteTimeCounter = coyoteTimeDuration;
    }

    
    /// Actualiza el contador del "Coyote Time" en cada frame, reduciéndolo según el tiempo transcurrido.
    /// Si el contador llega a cero, el "Coyote Time" expira y ya no permite saltar.
  
    public void UpdateCoyoteTime()
    {
        coyoteTimeCounter -= Time.deltaTime; // Resta el tiempo transcurrido desde el último frame.
    }
}
