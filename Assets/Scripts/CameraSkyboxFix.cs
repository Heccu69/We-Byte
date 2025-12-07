using UnityEngine;

/// <summary>
/// Автоматически настраивает камеру для отображения skybox
/// Добавьте этот скрипт на Main Camera если skybox не виден в игре
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraSkyboxFix : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Автоматически исправить настройки камеры при старте")]
    public bool autoFixOnStart = true;
    
    [Tooltip("Clear Flags для камеры")]
    public CameraClearFlags clearFlags = CameraClearFlags.Skybox;
    
    [Header("Background Settings")]
    [Tooltip("Цвет фона (используется если Clear Flags = Solid Color)")]
    public Color backgroundColor = new Color(0.19f, 0.3f, 0.47f);
    
    private Camera cameraComponent;
    
    void Awake()
    {
        cameraComponent = GetComponent<Camera>();
        
        if (autoFixOnStart)
        {
            FixCameraSettings();
        }
    }
    
    /// <summary>
    /// Исправляет настройки камеры для отображения skybox
    /// </summary>
    public void FixCameraSettings()
    {
        if (cameraComponent == null)
        {
            cameraComponent = GetComponent<Camera>();
        }
        
        // Устанавливаем Clear Flags на Skybox
        cameraComponent.clearFlags = clearFlags;
        
        // Устанавливаем цвет фона на случай если skybox не назначен
        cameraComponent.backgroundColor = backgroundColor;
        
        Debug.Log($"Camera '{gameObject.name}' настроена для отображения skybox. Clear Flags: {clearFlags}");
    }
    
    void OnValidate()
    {
        // Применяем настройки в редакторе при изменении параметров
        if (cameraComponent == null)
        {
            cameraComponent = GetComponent<Camera>();
        }
        
        if (cameraComponent != null)
        {
            cameraComponent.clearFlags = clearFlags;
            cameraComponent.backgroundColor = backgroundColor;
        }
    }
}
