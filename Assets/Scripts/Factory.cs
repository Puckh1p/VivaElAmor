// Factory.cs
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private GameObject productAPrefab;
    [SerializeField] private GameObject productBPrefab;

    public GameObject CreateProduct(string type, Vector3 spawnPosition)
    {
        GameObject instance = null;

        switch (type)
        {
            case "A":
                if (productAPrefab != null)
                {
                    instance = Instantiate(productAPrefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("Product A Prefab no está asignado en el Inspector.");
                }
                break;

            case "B":
                if (productBPrefab != null)
                {
                    instance = Instantiate(productBPrefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("Product B Prefab no está asignado en el Inspector.");
                }
                break;

            default:
                Debug.LogError("Tipo de producto desconocido: " + type);
                break;
        }

        if (instance != null)
        {
            IProduct product = instance.GetComponent<IProduct>();
            if (product != null)
            {
                product.Initialize();
            }
            else
            {
                Debug.LogError("El prefab '" + instance.name + "' no tiene un componente que implemente IProduct.");
            }
        }

        return instance;
    }
}
