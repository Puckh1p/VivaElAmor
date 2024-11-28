using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Factory factory; // Asignar la Factory desde el Inspector
    [SerializeField] private int maxFireballs = 7; // Máximo de instancias
    [SerializeField] private float spawnInterval = 1f; // Intervalo de generación

    private List<GameObject> fireballs = new List<GameObject>(); // Lista para rastrear bolas activas

    private void Start()
    {
        if (factory == null)
        {
            Debug.LogError("Factory no está asignada en el Inspector.");
            return;
        }

        InvokeRepeating(nameof(SpawnFireball), 0f, spawnInterval);
    }

    private void SpawnFireball()
    {
        // Elimina referencias a objetos destruidos
        fireballs.RemoveAll(fireball => fireball == null);

        // Verifica si ya se alcanzó el número máximo de bolas de fuego
        if (fireballs.Count >= maxFireballs) return;

        // Crear una nueva bola de fuego
        GameObject fireball = factory.CreateProduct("A", transform.position);

        // Asegúrate de añadirla a la lista
        if (fireball != null)
        {
            fireballs.Add(fireball);
        }
    }
}
