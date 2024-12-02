using UnityEngine;
using System.Collections.Generic; // Para usar listas

// Este script genera productos en intervalos definidos
public class Spawner : MonoBehaviour
{
    // Campo para asignar una referencia a un objeto que implemente IFactoryInterface
    [SerializeField] private MonoBehaviour factoryInterface;

    // Máximo número de productos que pueden estar activos simultáneamente
    [SerializeField] private int maxFireballs = 7;

    // Intervalo de tiempo (en segundos) entre cada generación
    [SerializeField] private float spawnInterval = 1f;

    // Referencia interna a la fábrica a través de la interfaz
    private IFactoryInterface factory;

    // Lista para rastrear los productos generados
    private List<GameObject> fireballs = new List<GameObject>();

    // Método que se llama al inicio
    private void Start()
    {
        // Convierte factoryInterface a un objeto que implemente IFactoryInterface
        factory = factoryInterface as IFactoryInterface;

        // Si factory no implementa la interfaz, muestra un error
        if (factory == null)
        {
            Debug.LogError("El objeto asignado no implementa IFactoryInterface.");
            return;
        }

        // Llama repetidamente al método SpawnFireball en intervalos definidos
        InvokeRepeating(nameof(SpawnFireball), 0f, spawnInterval);
    }

    // Método que genera un producto
    private void SpawnFireball()
    {
        // Limpia la lista eliminando referencias a objetos destruidos
        fireballs.RemoveAll(fireball => fireball == null);

        // Si ya se alcanzó el número máximo de productos, no genera más
        if (fireballs.Count >= maxFireballs) return;

        // Solicita un producto de tipo "A" a la fábrica
        GameObject fireball = factory.RequestProduct("A", transform.position);

        // Si el producto fue generado, lo añade a la lista
        if (fireball != null)
        {
            fireballs.Add(fireball);
        }
    }
}
