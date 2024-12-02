using UnityEngine;
using System.Collections.Generic; // Para usar listas

// Este script genera productos en intervalos definidos
public class Spawner : MonoBehaviour
{
    // Campo para asignar una referencia a un objeto que implemente IFactoryInterface
    [SerializeField] private MonoBehaviour factoryInterface;

    // M�ximo n�mero de productos que pueden estar activos simult�neamente
    [SerializeField] private int maxFireballs = 7;

    // Intervalo de tiempo (en segundos) entre cada generaci�n
    [SerializeField] private float spawnInterval = 1f;

    // Referencia interna a la f�brica a trav�s de la interfaz
    private IFactoryInterface factory;

    // Lista para rastrear los productos generados
    private List<GameObject> fireballs = new List<GameObject>();

    // M�todo que se llama al inicio
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

        // Llama repetidamente al m�todo SpawnFireball en intervalos definidos
        InvokeRepeating(nameof(SpawnFireball), 0f, spawnInterval);
    }

    // M�todo que genera un producto
    private void SpawnFireball()
    {
        // Limpia la lista eliminando referencias a objetos destruidos
        fireballs.RemoveAll(fireball => fireball == null);

        // Si ya se alcanz� el n�mero m�ximo de productos, no genera m�s
        if (fireballs.Count >= maxFireballs) return;

        // Solicita un producto de tipo "A" a la f�brica
        GameObject fireball = factory.RequestProduct("A", transform.position);

        // Si el producto fue generado, lo a�ade a la lista
        if (fireball != null)
        {
            fireballs.Add(fireball);
        }
    }
}
