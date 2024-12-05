using UnityEngine;

public class HUD : MonoBehaviour, IObserver
{
    public GameObject[] vidas;

    public void OnNotify(int vidasRestantes)
    {
        for (int i = 0; i < vidas.Length; i++)
        {
            vidas[i].SetActive(i < vidasRestantes);
        }
    }

    private void Start()
    {
        // Registra el HUD como observador del GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddObserver(this);
        }
    }

    private void OnDestroy()
    {
        // Remueve el HUD como observador al destruirse
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RemoveObserver(this);
        }
    }
}
