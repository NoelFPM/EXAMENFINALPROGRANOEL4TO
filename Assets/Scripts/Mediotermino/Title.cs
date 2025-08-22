using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void Jugar()
    {
        
        SceneManager.LoadScene("MedioTermino");
        AudioManager.Instance.Stop("Title");
    }

    public void Creditos()
    {
        SceneManager.LoadScene("Credits");
    }
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Title");
    }

    public void Save()
    {
        SceneManager.LoadScene("Menu");
    }

}

