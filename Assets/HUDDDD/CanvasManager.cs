using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    public void ToggleCanvas()
    {
        if (canvas != null)
        {
            canvas.SetActive(!canvas.activeSelf);
            Debug.Log("Canvas estado cambiado a: " + canvas.activeSelf);
        }
        else
        {
            Debug.LogError("Canvas no asignado en el Inspector.");
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCanvas();
        }
    }

}
