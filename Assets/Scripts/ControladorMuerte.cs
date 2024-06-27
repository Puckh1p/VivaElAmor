using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorMuerte : MonoBehaviour
{
    [SerializeField] private Button botonRegresar; // Referencia al botón

    private void Start()
    {
        // Asegúrate de que el botón esté asignado
        if (botonRegresar != null)
        {
            botonRegresar.onClick.AddListener(RegresarAScenaPrincipal);
        }
    }

    // Método para cargar la escena principal
    private void RegresarAScenaPrincipal()
    {
        SceneManager.LoadScene("IU");
    }
}
