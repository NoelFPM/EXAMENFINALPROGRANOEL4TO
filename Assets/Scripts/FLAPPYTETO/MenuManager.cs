using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject createAccountPanel;
    [SerializeField] private GameObject leaderboardPanel;

    [Header("Error Message")]
    [SerializeField] private GameObject errorMessagePanel;
    [SerializeField] private TMP_Text errorMessageText;
    [SerializeField] private float errorDisplayTime = 2f;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game"; 

    private bool isLoggedIn = false;

    private void Start()
    {
        
        ShowMainMenu();

        
        if (PlayfabManager.instance != null && !string.IsNullOrEmpty(PlayfabManager.instance.GetPlayerID()))
        {
            isLoggedIn = true;
        }
    }



    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        loginPanel.SetActive(false);
        createAccountPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        errorMessagePanel.SetActive(false);
    }

    public void ShowLoginPanel()
    {
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(true);
        createAccountPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }

    public void ShowCreateAccountPanel()
    {
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(false);
        createAccountPanel.SetActive(true);
        leaderboardPanel.SetActive(false);
    }

    public void ShowLeaderboardPanel()
    {
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(false);
        createAccountPanel.SetActive(false);
        leaderboardPanel.SetActive(true);

        // Cargar el leaderboard
        if (PlayfabManager.instance != null)
        {
            PlayfabManager.instance.GetLeaderboard();
        }
    }



    public void OnPlayButtonClicked()
    {
        if (isLoggedIn)
        {
           
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            
            ShowErrorMessage("¡Debes iniciar sesion primero!");
        }
    }



    public void OnLoginSuccessful()
    {
        isLoggedIn = true;
        ShowMainMenu();
        Debug.Log("Login exitoso! Ahora puedes jugar.");
    }

    public void OnCreateAccountSuccessful()
    {
        isLoggedIn = true;
        ShowMainMenu();
        Debug.Log("Cuenta creada! Ahora puedes jugar.");
    }

    // ========================= MENSAJES DE ERROR =========================

    private void ShowErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessagePanel.SetActive(true);
        Invoke(nameof(HideErrorMessage), errorDisplayTime);
    }

    private void HideErrorMessage()
    {
        errorMessagePanel.SetActive(false);
    }


    public void BackToMainMenu()
    {
        ShowMainMenu();
    }
}