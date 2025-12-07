using UnityEngine;

/// <summary>
/// Создает эффект света, проходящего через окно
/// Добавляет реалистичные тени и объемный свет
/// </summary>
[RequireComponent(typeof(Light))]
public class WindowLightEffect : MonoBehaviour
{
    [Header("Light Animation")]
    [Tooltip("Легкое мерцание света (имитация облаков)")]
    public bool enableFlicker = true;
    
    [Tooltip("Скорость мерцания")]
    public float flickerSpeed = 0.5f;
    
    [Tooltip("Сила мерцания (0-1)")]
    [Range(0f, 0.3f)]
    public float flickerAmount = 0.05f;
    
    [Header("Light Rays")]
    [Tooltip("Включить эффект световых лучей (God Rays)")]
    public bool enableLightRays = true;
    
    [Tooltip("Интенсивность световых лучей")]
    [Range(0f, 1f)]
    public float rayIntensity = 0.3f;
    
    private Light lightComponent;
    private float baseIntensity;
    private float flickerOffset;
    
    void Start()
    {
        lightComponent = GetComponent<Light>();
        baseIntensity = lightComponent.intensity;
        flickerOffset = Random.Range(0f, 100f);
        
        // Настраиваем свет для реалистичного эффекта окна
        ConfigureWindowLight();
    }
    
    void Update()
    {
        if (enableFlicker && lightComponent != null)
        {
            // Создаем эффект мерцания света
            float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed + flickerOffset, 0f);
            flicker = (flicker - 0.5f) * flickerAmount;
            lightComponent.intensity = baseIntensity + flicker;
        }
    }
    
    /// <summary>
    /// Настраивает параметры света для эффекта окна
    /// </summary>
    private void ConfigureWindowLight()
    {
        if (lightComponent == null) return;
        
        // Используем Spot Light для направленного света из окна
        if (lightComponent.type == LightType.Spot)
        {
            lightComponent.spotAngle = 60f; // Угол конуса света
            lightComponent.innerSpotAngle = 30f; // Внутренний угол
        }
        
        // Включаем тени для реалистичности
        lightComponent.shadows = LightShadows.Soft;
        lightComponent.shadowStrength = 0.8f;
        lightComponent.shadowBias = 0.05f;
        lightComponent.shadowNormalBias = 0.4f;
        
        // Настраиваем дальность света
        lightComponent.range = 20f;
        
        // Включаем cookie для имитации рамы окна (опционально)
        // lightComponent.cookie = windowFrameTexture; // Назначить в инспекторе
    }
    
    /// <summary>
    /// Обновить базовую интенсивность (вызывается из WeatherController)
    /// </summary>
    public void UpdateBaseIntensity(float newIntensity)
    {
        baseIntensity = newIntensity;
    }
    
    /// <summary>
    /// Включить/выключить эффект мерцания
    /// </summary>
    public void SetFlickerEnabled(bool enabled)
    {
        enableFlicker = enabled;
        if (!enabled && lightComponent != null)
        {
            lightComponent.intensity = baseIntensity;
        }
    }
}
