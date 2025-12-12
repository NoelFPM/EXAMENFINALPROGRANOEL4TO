using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    [Header("Weather API")]
    private string apiKey = "7c97130f6988e7496b7693cf8be2c4d9"; 
    private string[] cities = { "Tokyo", "London", "Paris", "New York", "Sydney", "Berlin", "Moscow", "Dubai", "Toronto", "Mexico" };

    [Header("Visual Effects")]
    [SerializeField] private SpriteRenderer backgroundSprite; 
    [SerializeField] private Camera mainCamera;

    private Color originalBackgroundColor;
    private Color originalCameraColor;

    private void Start()
    {
        
        if (backgroundSprite != null)
            originalBackgroundColor = backgroundSprite.color;

        if (mainCamera != null)
            originalCameraColor = mainCamera.backgroundColor;

        GetWeatherForRandomCity();
    }

    private void GetWeatherForRandomCity()
    {
        string randomCity = cities[Random.Range(0, cities.Length)];
        StartCoroutine(GetWeather(randomCity));
    }

    private IEnumerator GetWeather(string city)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            WeatherData data = JsonUtility.FromJson<WeatherData>(request.downloadHandler.text);
            ApplyWeatherEffects(data.weather[0].main);
            Debug.Log($"Clima en {city}: {data.weather[0].main}");
        }
        else
        {
            Debug.LogError("Error al obtener clima: " + request.error);
            ApplyWeatherEffects("Clear"); 
        }
    }

    private void ApplyWeatherEffects(string weatherCondition)
    {
        switch (weatherCondition.ToLower())
        {
            case "clear":
               
                if (backgroundSprite) backgroundSprite.color = new Color(1f, 1f, 1f, 1f); 
                if (mainCamera) mainCamera.backgroundColor = new Color(0.53f, 0.81f, 0.92f); 
                break;

            case "clouds":
                // Nublado - Más gris
                if (backgroundSprite) backgroundSprite.color = new Color(0.7f, 0.7f, 0.7f, 1f); 
                if (mainCamera) mainCamera.backgroundColor = new Color(0.6f, 0.6f, 0.7f);
                break;

            case "rain":
            case "drizzle":
                // Lluvia - Azul oscuro
                if (backgroundSprite) backgroundSprite.color = new Color(0.5f, 0.5f, 0.7f, 1f); 
                if (mainCamera) mainCamera.backgroundColor = new Color(0.3f, 0.3f, 0.5f);
                break;

            case "snow":
                // Nieve - Más blanco/brillante
                if (backgroundSprite) backgroundSprite.color = new Color(1f, 1f, 1.2f, 1f); 
                if (mainCamera) mainCamera.backgroundColor = new Color(0.85f, 0.9f, 1f);
                break;

            case "thunderstorm":
                // Tormenta - Muy oscuro
                if (backgroundSprite) backgroundSprite.color = new Color(0.3f, 0.3f, 0.4f, 1f); 
                if (mainCamera) mainCamera.backgroundColor = new Color(0.2f, 0.2f, 0.3f);
                break;

            case "mist":
            case "fog":
                // Niebla - Gris claro
                if (backgroundSprite) backgroundSprite.color = new Color(0.8f, 0.8f, 0.85f, 1f);
                if (mainCamera) mainCamera.backgroundColor = new Color(0.7f, 0.7f, 0.75f);
                break;

            default:
                // Por defecto - Normal
                if (backgroundSprite) backgroundSprite.color = originalBackgroundColor;
                if (mainCamera) mainCamera.backgroundColor = originalCameraColor;
                break;
        }
    }

    [System.Serializable]
    public class WeatherData
    {
        public Weather[] weather;
    }

    [System.Serializable]
    public class Weather
    {
        public string main;
        public string description;
    }
}