using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorMuerte : MonoBehaviour
{
    [SerializeField] private Button botonRegresar; // Referencia al bot�n

    private void Start()
    {
        // Aseg�rate de que el bot�n est� asignado
        if (botonRegresar != null)
        {
            botonRegresar.onClick.AddListener(RegresarAScenaPrincipal);
        }
    }

    // M�todo para cargar la escena principal
    private void RegresarAScenaPrincipal()
    {
        SceneManager.LoadScene("IU");
    }
}
