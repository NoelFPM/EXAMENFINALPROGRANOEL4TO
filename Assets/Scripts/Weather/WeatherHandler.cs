using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using SimpleJSON;



public class WeatherHandler : MonoBehaviour
{
  
    public event System.Action OnWeatherUpdated;

    [Header("Configuración de Ubicación")]
    [SerializeField] private float lat = 25.6866f; // Default: Monterrey, MX
    [SerializeField] private float lon = -100.3175f; // Default: Monterrey, MX

    [Header("Datos del Clima")]
    [SerializeField] private WeatherData weatherData;


    public float CurrentTemp => weatherData.temp;
    public string CurrentWeatherDescription => weatherData.weatherDescription;

    private string apiKey = "7c97130f6988e7496b7693cf8be2c4d9";
    private string url;
    private string jsonRAW;

    private const float updateInterval = 600f; //actualiza el clima

    void Start()
    {
        url = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&exclude=minutely,hourly,daily&appid={apiKey}&units=metric";
        StartCoroutine(UpdateWeatherRoutine()); 
    }

    IEnumerator UpdateWeatherRoutine()
    {
        while (true) 
        {
            yield return StartCoroutine(FetchWeather());
            yield return new WaitForSeconds(updateInterval);
        }
    }

    IEnumerator FetchWeather()
    {
        UnityWebRequest request = new UnityWebRequest(url);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[WeatherHandler] Error al obtener datos: {request.error}");
        }
        else
        {
            jsonRAW = request.downloadHandler.text;
            Debug.Log($"[WeatherHandler] Datos obtenidos. JSON RAW: {jsonRAW.Substring(0, Mathf.Min(jsonRAW.Length, 150))}...");
            ReadJson();
            OnWeatherUpdated?.Invoke();
        }
    }

    private void ReadJson()
    {
        var weatherJson = JSON.Parse(jsonRAW);

        weatherData.timeZone = weatherJson["timezone"].Value;

 
        if (float.TryParse(weatherJson["current"]["temp"].Value, out float tempValue))
        {
            weatherData.temp = tempValue;
        }
        else
        {
            Debug.LogError("[WeatherHandler] No se pudo parsear la temperatura.");
            weatherData.temp = 0f; 
        }

        weatherData.weatherDescription = weatherJson["current"]["weather"][0]["description"].Value;

        Debug.Log($"[WeatherHandler] Clima actualizado: {weatherData.weatherDescription}, Temp: {weatherData.temp}°C");
    }
}