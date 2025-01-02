using UnityEngine;

public class CoyoteTimeController : MonoBehaviour
{
    [SerializeField] private float coyoteTimeDuration = 0.15f;
    private float coyoteTimeCounter;

    public bool PuedeSaltar => coyoteTimeCounter > 0f;  // Propiedad que indica si el coyote time sigue activo

    public void ResetCoyoteTime()
    {
        coyoteTimeCounter = coyoteTimeDuration;
    }

    public void UpdateCoyoteTime()
    {
        coyoteTimeCounter -= Time.deltaTime;  // Cuenta regresiva
    }
}
