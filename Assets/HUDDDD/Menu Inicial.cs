using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour

{
    [SerializeField] private AudioClip startSound;

     public void Jugar()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.Instance.PlaySound(startSound);
        AudioManager.Instance.GetIsMusicPlaying();
   }

   public void Salir()
   {
    Debug.Log("Salir...");
    Application.Quit();
   }
}
