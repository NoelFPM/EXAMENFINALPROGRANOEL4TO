using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Configuracion del Juego")]
    [SerializeField] private int enemiesToDefeat = 10;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private int enemiesDefeated = 0;
    private bool gameCompleted = false;

    private void Start()
    {
        UpdateUI();
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }

    public void EnemyDefeated()
    {
        if (gameCompleted) return;

        enemiesDefeated++;
        UpdateUI();

        if (enemiesDefeated >= enemiesToDefeat)
        {
            GameCompleted();
        }
    }

    private void UpdateUI()
    {
        if (enemiesText != null)
            enemiesText.text = "Enemigos Derrotados: " + enemiesDefeated + "/" + enemiesToDefeat;

        if (objectiveText != null)
            objectiveText.text = "Objetivo: Derrotar " + enemiesToDefeat + " enemigos";
    }

    private void GameCompleted()
    {
        gameCompleted = true;
        if (gameOverText != null)
        {
            gameOverText.text = "¡VICTORIA!\nVolviendo al menú...";
            gameOverText.gameObject.SetActive(true);
        }

        // Volver al menú 
        Invoke("ReturnToMenu", 3f);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("5toMedioTitle");
    }
}