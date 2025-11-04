using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Botones del Menú")]
    [SerializeField] private TextMeshProUGUI tituloText;
    [SerializeField] private GameObject botonJugar;
    [SerializeField] private GameObject botonSalir;

    private void Start()
    {
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

  
    public void Jugar()
    {
        SceneManager.LoadScene("5toMedio");
    }


    public void Salir()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}