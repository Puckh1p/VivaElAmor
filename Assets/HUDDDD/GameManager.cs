using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public HUD hud; // Referencia al HUD

    private int vidas = 3; // N�mero de vidas del jugador

    // Lista de observadores
    private List<IObserver> observers = new List<IObserver>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // M�todos para gestionar observadores
    public void AddObserver(IObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    // M�todo para notificar a todos los observadores
    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(vidas);
        }
    }

    // M�todo para manejar la p�rdida de vida del jugador
    public void PerderVida()
    {
        if (vidas > 0)
        {
            vidas--;
            NotifyObservers(); // Notifica a los observadores sobre el cambio

            if (vidas <= 0)
            {
                SceneManager.LoadScene("Muerte"); // Cambia a la escena de muerte
            }
        }
    }
}
