using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter Instance;

    public int totalEnemies = 20;
    public TextMeshProUGUI enemyText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Esto se ejecuta CADA VEZ que se recarga la escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reconectar el texto automáticamente
        ReconectarTexto();
        UpdateUI();
    }

    private void Start()
    {
        ReconectarTexto();
        UpdateUI();
    }

    // Método para encontrar el texto automáticamente
    private void ReconectarTexto()
    {
        if (enemyText == null)
        {
            // Buscar cualquier TextMeshPro en la escena
            TextMeshProUGUI[] todosTextos = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);

            foreach (TextMeshProUGUI texto in todosTextos)
            {
                if (texto.gameObject.name.Contains("Enemy") || texto.text.Contains("Enemigos"))
                {
                    enemyText = texto;
                    Debug.Log("Texto reconectado: " + texto.name);
                    break;
                }
            }
        }
    }

    public void EnemyKilled()
    {
        totalEnemies--;
        if (totalEnemies < 0) totalEnemies = 0;
        UpdateUI();

        if (totalEnemies == 0)
        {
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene("Victory");
        }
    }

    void UpdateUI()
    {
        if (enemyText != null)
        {
            enemyText.text = "Enemigos restantes: " + totalEnemies;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}