// ConcreteProductA.cs
using UnityEngine;

public class ConcreteProductA : MonoBehaviour, IProduct
{
    public void Initialize()
    {
        Debug.Log("Producto A creado e inicializado.");
        // L�gica espec�fica para ProductA
    }
}
