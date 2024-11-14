
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Factory factory;

    private void Start()
    {
        factory = FindObjectOfType<Factory>();

        if (factory == null)
        {
            Debug.LogError("No se encontr� una Factory en la escena. Aseg�rate de que el objeto con el script Factory est� presente y activo en la escena.");
            return;
        }

        // Crear productos si la Factory se encuentra
        factory.CreateProduct("A");
        factory.CreateProduct("B");
    }
}
