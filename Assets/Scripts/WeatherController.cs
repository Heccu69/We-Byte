using System.Collections;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [Header("Weather Transition Settings")]
    [Tooltip("Длительность смены погоды в секундах (600 = 10 минут)")]
    public float transitionDuration = 600f; // 10 минут
    
    [Header("Lighting Settings")]
    [Tooltip("Основной источник света (Directional Light)")]
    public Light mainLight;
    
    [Tooltip("Свет из окна")]
    public Light windowLight;
    
    [Header("Morning Settings (Утро)")]
    [Tooltip("Цвет света утром")]
    public Color morningLightColor = new Color(1f, 0.95f, 0.8f); // Теплый утренний свет
    
    [Tooltip("Интенсивность света утром")]
    public float morningLightIntensity = 1.2f;
    
    [Tooltip("Цвет ambient утром")]
    public Color morningAmbientColor = new Color(0.7f, 0.8f, 0.9f);
    
    [Tooltip("Цвет света из окна утром")]
    public Color morningWindowColor = new Color(1f, 0.95f, 0.85f);
    
    [Tooltip("Интенсивность света из окна утром")]
    public float morningWindowIntensity = 2f;
    
    [Header("Evening Settings (Поздний вечер)")]
    [Tooltip("Цвет света вечером")]
    public Color eveningLightColor = new Color(1f, 0.6f, 0.3f); // Оранжевый закат
    
    [Tooltip("Интенсивность света вечером")]
    public float eveningLightIntensity = 0.4f;
    
    [Tooltip("Цвет ambient вечером")]
    public Color eveningAmbientColor = new Color(0.3f, 0.3f, 0.5f);
    
    [Tooltip("Цвет света из окна вечером")]
    public Color eveningWindowColor = new Color(1f, 0.5f, 0.2f);
    
    [Tooltip("Интенсивность света из окна вечером")]
    public float eveningWindowIntensity = 1f;
    
    [Header("Skybox Settings")]
    [Tooltip("Skybox материал для утра")]
    public Material morningSkybox;
    
    [Tooltip("Skybox материал для вечера")]
    public Material eveningSkybox;
    
    [Header("Fog Settings")]
    [Tooltip("Использовать туман")]
    public bool useFog = true;
    
    [Tooltip("Цвет тумана утром")]
    public Color morningFogColor = new Color(0.8f, 0.9f, 1f);
    
    [Tooltip("Цвет тумана вечером")]
    public Color eveningFogColor = new Color(0.5f, 0.4f, 0.6f);
    
    [Tooltip("Плотность тумана утром")]
    public float morningFogDensity = 0.001f;
    
    [Tooltip("Плотность тумана вечером")]
    public float eveningFogDensity = 0.005f;
    
    [Header("Auto Start")]
    [Tooltip("Автоматически начать смену погоды при старте")]
    public bool autoStart = true;
    
    private float currentTime = 0f;
    private bool isTransitioning = false;
    
    void Start()
    {
        // Устанавливаем начальные параметры (утро)
        SetMorningWeather();
        
        if (autoStart)
        {
            StartWeatherTransition();
        }
    }
    
    void Update()
    {
        if (isTransitioning)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / transitionDuration);
            
            UpdateWeather(progress);
            
            if (progress >= 1f)
            {
                isTransitioning = false;
                Debug.Log("Weather transition completed!");
            }
        }
    }
    
    /// <summary>
    /// Начать смену погоды
    /// </summary>
    public void StartWeatherTransition()
    {
        currentTime = 0f;
        isTransitioning = true;
        Debug.Log("Weather transition started! Duration: " + transitionDuration + " seconds");
    }
    
    /// <summary>
    /// Остановить смену погоды
    /// </summary>
    public void StopWeatherTransition()
    {
        isTransitioning = false;
    }
    
    /// <summary>
    /// Установить утреннюю погоду
    /// </summary>
    public void SetMorningWeather()
    {
        UpdateWeather(0f);
    }
    
    /// <summary>
    /// Установить вечернюю погоду
    /// </summary>
    public void SetEveningWeather()
    {
        UpdateWeather(1f);
    }
    
    /// <summary>
    /// Обновить погоду на основе прогресса (0 = утро, 1 = вечер)
    /// </summary>
    private void UpdateWeather(float progress)
    {
        // Обновляем основной свет
        if (mainLight != null)
        {
            mainLight.color = Color.Lerp(morningLightColor, eveningLightColor, progress);
            mainLight.intensity = Mathf.Lerp(morningLightIntensity, eveningLightIntensity, progress);
            
            // Поворачиваем свет (солнце садится)
            float angle = Mathf.Lerp(50f, 10f, progress); // От 50° до 10° над горизонтом
            mainLight.transform.rotation = Quaternion.Euler(angle, -30f, 0f);
        }
        
        // Обновляем свет из окна
        if (windowLight != null)
        {
            windowLight.color = Color.Lerp(morningWindowColor, eveningWindowColor, progress);
            windowLight.intensity = Mathf.Lerp(morningWindowIntensity, eveningWindowIntensity, progress);
        }
        
        // Обновляем ambient освещение
        RenderSettings.ambientLight = Color.Lerp(morningAmbientColor, eveningAmbientColor, progress);
        
        // Обновляем skybox
        if (morningSkybox != null && eveningSkybox != null)
        {
            // Плавный переход между skybox через смешивание
            RenderSettings.skybox = progress < 0.5f ? morningSkybox : eveningSkybox;
            
            // Если нужен более плавный переход, можно использовать Skybox Blend материал
            // или создать свой шейдер для смешивания
        }
        
        // Обновляем туман
        if (useFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.Lerp(morningFogColor, eveningFogColor, progress);
            RenderSettings.fogDensity = Mathf.Lerp(morningFogDensity, eveningFogDensity, progress);
            RenderSettings.fogMode = FogMode.ExponentialSquared;
        }
        
        // Обновляем reflection intensity
        RenderSettings.reflectionIntensity = Mathf.Lerp(1f, 0.5f, progress);
    }
    
    /// <summary>
    /// Установить прогресс вручную (для тестирования)
    /// </summary>
    public void SetProgress(float progress)
    {
        UpdateWeather(Mathf.Clamp01(progress));
    }
    
    void OnValidate()
    {
        // Обновляем погоду в редакторе при изменении параметров
        if (!Application.isPlaying && mainLight != null)
        {
            UpdateWeather(0f);
        }
    }
}
