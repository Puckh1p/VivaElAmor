// ConcreteProductA.cs
using UnityEngine;

public class ConcreteProductA : MonoBehaviour, IProduct
{
    public void Initialize()
    {
        Debug.Log("Producto A creado e inicializado.");
        // Lógica específica para ProductA
    }
}
