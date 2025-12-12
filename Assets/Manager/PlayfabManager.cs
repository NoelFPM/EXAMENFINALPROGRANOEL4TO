using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.Networking;
using System.Collections;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance; 

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
 
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "11C408";
        }
    }

    [Header("LOGIN")]
    [SerializeField] private TMP_InputField loginEmail;
    [SerializeField] private TMP_InputField loginPassword;
    [SerializeField] private UnityEvent onLoginSuccess;

    [Header("CREATE ACCOUNT")]
    [SerializeField] private TMP_InputField CA_EmailText;
    [SerializeField] private TMP_InputField CA_UsernameText;
    [SerializeField] private TMP_InputField CA_PasswordText;
    [SerializeField] private TMP_InputField CA_ConfirmPassword;
    [SerializeField] private TMP_InputField CA_AvatarUrl;
    [SerializeField] private UnityEvent onCreateAccountSuccess;

    [Header("User Info")]
    [SerializeField] private Image playerProfilePic;
    [SerializeField] private TMP_Text playerDisplayName;
    private string userPlayerfabID;
    private string playerAvatarUrl; 

    [Header("LEADERBOARD")]
    [SerializeField] private GameObject leaderboardRowPrefab;
    [SerializeField] private Transform leaderboardContentParent;


    public void SetLeaderboardParent(Transform parent)
    {
        
        if (parent != null)
        {
            leaderboardContentParent = parent;
            Debug.Log("Leaderboard Content Parent reasignado.");
        }
    }

    public string GetPlayerID() => userPlayerfabID;
    public string GetPlayerAvatarUrl() => playerAvatarUrl;
    public string GetPlayerDisplayName() => playerDisplayName != null ? playerDisplayName.text : "";

    

    public void CreateAccount()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
        {
            Email = CA_EmailText.text,
            Username = CA_UsernameText.text,
            DisplayName = CA_UsernameText.text,
            Password = CA_PasswordText.text,
            RequireBothUsernameAndEmail = true
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnCreateAccountSuccess, OnError);
    }

    private void OnCreateAccountSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Tu cuenta fue creada correctamente");
        userPlayerfabID = result.PlayFabId;
        onCreateAccountSuccess?.Invoke();
    }

    public void SetUserAvatar()
    {
        UpdateAvatarUrlRequest request = new UpdateAvatarUrlRequest
        {
            ImageUrl = CA_AvatarUrl.text,
        };

        PlayFabClientAPI.UpdateAvatarUrl(request, OnSetUserAvatarSuccess, OnError);
    }

    public void OnSetUserAvatarSuccess(EmptyResponse response)
    {
        Debug.Log("Avatar Configurado");
        playerAvatarUrl = CA_AvatarUrl.text;
        StartCoroutine(SetProfilePicOnCanvas(CA_AvatarUrl.text));
    }

    // ========================= LOGIN =========================

    public void LoginWithEmail()
    {
        LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest
        {
            Email = loginEmail.text,
            Password = loginPassword.text
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("¡Iniciaste sesión correctamente!");
        userPlayerfabID = result.PlayFabId;

        GetPlayerProfile();

        onLoginSuccess?.Invoke();
    }

    // ========================= USER PROFILE =========================

    public void GetPlayerProfile()
    {
        GetPlayerProfileRequest request = new GetPlayerProfileRequest
        {
            PlayFabId = userPlayerfabID
        };

        PlayFabClientAPI.GetPlayerProfile(request, OnGetPlayerProfileSuccess, OnError);
    }

    public void OnGetPlayerProfileSuccess(GetPlayerProfileResult result)
    {
        if (playerDisplayName != null)
            playerDisplayName.text = result.PlayerProfile.DisplayName;

        playerAvatarUrl = result.PlayerProfile.AvatarUrl;

        if (playerProfilePic != null && !string.IsNullOrEmpty(playerAvatarUrl))
            StartCoroutine(SetProfilePicOnCanvas(playerAvatarUrl));
    }

    // ========================= LEADERBOARD =========================

    public void GetLeaderboard()
    {
        GetLeaderboardRequest request = new GetLeaderboardRequest
        {
            StatisticName = "SCORE",
            StartPosition = 0,
            MaxResultsCount = 100,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true,
                ShowAvatarUrl = true 
            }
        };

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnError);
    }

    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log($"Leaderboard recibido con {result.Leaderboard.Count} jugadores");

       
        foreach (Transform child in leaderboardContentParent)
            Destroy(child.gameObject);

       
        int position = 1;
        foreach (var player in result.Leaderboard)
        {
            GameObject row = Instantiate(leaderboardRowPrefab, leaderboardContentParent);

            
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            if (texts.Length >= 2)
            {
                texts[0].text = $"#{position} - {player.DisplayName}";
                texts[1].text = player.StatValue.ToString();
            }

            
            Image avatarImage = null;
            Image[] images = row.GetComponentsInChildren<Image>();

            foreach (var img in images)
            {
                if (img.gameObject.name == "PlayerAvatar")
                {
                    avatarImage = img;
                    break;
                }
            }

            
            if (avatarImage != null)
            {
                if (!string.IsNullOrEmpty(player.Profile?.AvatarUrl))
                {
                    avatarImage.gameObject.SetActive(true);
                    StartCoroutine(LoadImage(player.Profile.AvatarUrl, avatarImage));

                   
                    if (player.PlayFabId == userPlayerfabID)
                    {
                       
                        Image rowBg = row.GetComponent<Image>();
                        if (rowBg != null)
                        {
                            rowBg.color = new Color(1f, 0.84f, 0f, 0.3f); 
                        }
                    }
                }
                else
                {
                   
                    avatarImage.gameObject.SetActive(true);
                    
                    StartCoroutine(LoadImage("https://i.pravatar.cc/150?img=" + position, avatarImage));
                }
            }

            position++;
        }
    }

    // ========================= SCORE =========================

    public void UpdateScore(int score)
    {
        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "SCORE",
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnPlayerStatsUpdateSuccess, OnError);
    }

    private void OnPlayerStatsUpdateSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Tu score se actualizó correctamente en PlayFab");
    }

    // ========================= AVATAR =========================

    private IEnumerator SetProfilePicOnCanvas(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            if (playerProfilePic != null)
                playerProfilePic.sprite = sprite;
        }
    }

    private IEnumerator LoadImage(string url, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            image.sprite = sprite;
        }
    }

    // ========================= CARGAR IMAGEN DESDE OTRA ESCENA =========================
    public void LoadPlayerAvatarToImage(Image targetImage)
    {
        if (!string.IsNullOrEmpty(playerAvatarUrl))
        {
            StartCoroutine(LoadImage(playerAvatarUrl, targetImage));
        }
    }

    // ========================= ERRORES ========================
    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }


}