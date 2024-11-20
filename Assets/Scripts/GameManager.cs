using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public HUD hud; // Referencia al HUD que controla las vidas en pantalla

    private int vidas = 3; // Número de vidas restantes

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el GameManager entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PerderVida()
    {
        if (hud == null)
        {
            Debug.LogError("HUD no asignado en GameManager.");
            return;
        }

        if (vidas > 0)
        {
            vidas -= 1; // Resta una vida
            Debug.Log("Vida restante: " + vidas);

            hud.DesactivarVida(vidas); // Actualiza el HUD para desactivar el corazón correspondiente

            if (vidas <= 0)
            {
                SceneManager.LoadScene("Muerte"); // Cambia a la escena de muerte si ya no hay vidas
            }
        }
    }
}
