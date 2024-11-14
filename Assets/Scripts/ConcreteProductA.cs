// ConcreteProductA.cs
using UnityEngine;

public class ConcreteProductA : MonoBehaviour, IProduct
{
    public void Initialize()
    {
        Debug.Log("ConcreteProductA creado e inicializado.");
        // Lógica específica para ProductA
    }
}
