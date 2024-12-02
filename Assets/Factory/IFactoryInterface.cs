using UnityEngine;

// Esta es una interfaz que define un contrato para cualquier clase que implemente IFactoryInterface
public interface IFactoryInterface
{
    // Método que las fábricas deben implementar para crear y devolver un GameObject
    GameObject RequestProduct(string type, Vector3 spawnPosition);
}
