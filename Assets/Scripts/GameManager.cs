using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public HUD hud;

    private int vidas = 3;

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
            Debug.LogError("HUD is not assigned in GameManager.");
            return;
        }

        if (vidas > 0)
        {
            vidas -= 1;
            hud.DesactivarVida(vidas);

            if (vidas <= 0)
            {
                SceneManager.LoadScene("Muerte");
            }
        }
    }
}
