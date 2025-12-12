using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject _gameOverCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        _gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;

       
        if (PlayfabManager.instance != null && Score.instance != null)
        {
            int finalScore = Score.instance.GetCurrentScore();
            PlayfabManager.instance.UpdateScore(finalScore);
            Debug.Log("Guardando puntaje en PlayFab: " + finalScore);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0); 
    }
}