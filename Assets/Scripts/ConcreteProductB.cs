// ConcreteProductB.cs
using UnityEngine;

public class ConcreteProductB : MonoBehaviour, IProduct
{
    public void Initialize()
    {
        Debug.Log("ConcreteProductB creado e inicializado.");
        // L�gica espec�fica para ProductB
    }
}
