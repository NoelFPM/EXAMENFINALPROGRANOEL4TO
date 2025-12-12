using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score instance;

    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private Image _playerProfileImage; 

    private int _score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _currentScoreText.text = _score.ToString();
        _highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        
        if (PlayfabManager.instance != null && _playerProfileImage != null)
        {
            PlayfabManager.instance.LoadPlayerAvatarToImage(_playerProfileImage);
        }

        UpdateHighScore();
    }

    public void UpdateScore()
    {
        _score++;
        _currentScoreText.text = _score.ToString();
        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        if (_score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", _score);
            _highScoreText.text = _score.ToString();
        }
    }

    
    public int GetCurrentScore()
    {
        return _score;
    }
}