using UnityEngine;

// La clase Factory implementa la interfaz IFactoryInterface
public class Factory : MonoBehaviour, IFactoryInterface
{
    // Prefab del producto A que se asigna desde el Inspector
    [SerializeField] private GameObject productAPrefab;

    // Prefab del producto B que se asigna desde el Inspector
    [SerializeField] private GameObject productBPrefab;

    // Este m�todo cumple el contrato definido en IFactoryInterface
    public GameObject RequestProduct(string type, Vector3 spawnPosition)
    {
        // Llama a CreateProduct para generar el producto
        return CreateProduct(type, spawnPosition);
    }

    // M�todo privado que crea un producto basado en el tipo solicitado
    private GameObject CreateProduct(string type, Vector3 spawnPosition)
    {
        GameObject instance = null; // Aqu� se almacenar� la instancia creada

        // Verifica el tipo de producto que se quiere crear
        switch (type)
        {
            case "A":
                // Si se solicita "A", intenta instanciar el prefab correspondiente
                if (productAPrefab != null)
                {
                    instance = Instantiate(productAPrefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("Product A Prefab no est� asignado en el Inspector.");
                }
                break;

            case "B":
                // Si se solicita "B", intenta instanciar el prefab correspondiente
                if (productBPrefab != null)
                {
                    instance = Instantiate(productBPrefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("Product B Prefab no est� asignado en el Inspector.");
                }
                break;

            default:
                // Si el tipo no es v�lido, muestra un mensaje de error
                Debug.LogError("Tipo de producto desconocido: " + type);
                break;
        }

        // Si se cre� una instancia, verifica si tiene un componente que implemente IProduct
        if (instance != null)
        {
            IProduct product = instance.GetComponent<IProduct>();
            if (product != null)
            {
                // Si lo tiene, llama al m�todo Initialize
                product.Initialize();
            }
            else
            {
                Debug.LogError("El prefab '" + instance.name + "' no tiene un componente que implemente IProduct.");
            }
        }

        // Devuelve la instancia creada (o null si no se cre�)
        return instance;
    }
}
