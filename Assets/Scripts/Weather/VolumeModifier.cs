using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeModifier : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Global Volume")]
    [SerializeField] private Volume globalVolume;

    [SerializeField] private WeatherHandler weatherHandler;
    [SerializeField] private float fadeSpeed = 0.5f;

    [Header("Color y Rango")]
    [SerializeField] private Color coldColor = new Color(0.2f, 0.4f, 1.0f, 1.0f);   // Azul = frío
    [SerializeField] private Color warmColor = new Color(1.0f, 0.2f, 0.2f, 1.0f);   // Rojo = calor
    //Normal pues no tiene nada
    private readonly Color normalColor = Color.black;

    private const float MAX_INTENSITY = 0.5f; 
    private const float MIN_INTENSITY = 0.0f; 

  
    private Vignette vignetteOverride;


    private float targetIntensity; //objetivo
    private Color targetColor;

    void Start()
    {

        if (globalVolume == null || globalVolume.profile == null || weatherHandler == null)
        {
            Debug.LogError("faltan referencias.");
            enabled = false;
            return;
        }


        if (!globalVolume.profile.TryGet<Vignette>(out vignetteOverride))
        {
            Debug.LogError("ERROR");
            enabled = false;
            return;
        }

       
        vignetteOverride.active = true;
        vignetteOverride.intensity.overrideState = true;
        vignetteOverride.color.overrideState = true;

       
        weatherHandler.OnWeatherUpdated += HandleWeatherUpdate;

        
        HandleWeatherUpdate();
    }

    void OnDestroy()
    {
      
        if (weatherHandler != null)
        {
            weatherHandler.OnWeatherUpdated -= HandleWeatherUpdate;
        }
    }

   
    private void HandleWeatherUpdate()
    {
        float currentTemp = weatherHandler.CurrentTemp;

      

        if (currentTemp < 20f)
        {
          
            targetColor = coldColor;

           
            float t = Mathf.InverseLerp(20f, 0f, currentTemp);
            targetIntensity = Mathf.Lerp(MIN_INTENSITY, MAX_INTENSITY, t);

            Debug.Log($"FRÍO Temp: {currentTemp:F1}°C.  ({targetIntensity:F2}).");

        }
        else if (currentTemp >= 20f && currentTemp <= 30f)
        {
          
            targetColor = normalColor;
            targetIntensity = MIN_INTENSITY;

            Debug.Log($"NORMAL. Temp: {currentTemp:F1}°C.");
        }
        else 
        {
           
            targetColor = warmColor;

            float t = Mathf.InverseLerp(30f, 40f, currentTemp);
            targetIntensity = Mathf.Lerp(MIN_INTENSITY, MAX_INTENSITY, t);

            Debug.Log($"CALOR  Temp: {currentTemp:F1}°C.  ({targetIntensity:F2}).");
        }
    }

    void Update()
    {
       

        // 1. Interpolación de Intensidad 
        float currentIntensity = vignetteOverride.intensity.value;

        float newIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * fadeSpeed);
        vignetteOverride.intensity.value = newIntensity;

        // 2. Interpolación de Color
        Color currentColor = vignetteOverride.color.value;
        Color newColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * fadeSpeed * 2f);
        vignetteOverride.color.value = newColor;
    }
}