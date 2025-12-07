using UnityEngine;
using TMPro;

/// <summary>
/// Компонент для автоматической локализации текста
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Ключ перевода из LocalizationSystem")]
    public string localizationKey;
    
    [Tooltip("Использовать форматирование (например, для вставки чисел)")]
    public bool useFormatting = false;
    
    private TextMeshProUGUI textComponent;
    
    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }
    
    void Start()
    {
        UpdateText();
    }
    
    /// <summary>
    /// Обновить текст согласно текущему языку
    /// </summary>
    public void UpdateText()
    {
        if (LocalizationSystem.Instance == null)
        {
            Debug.LogWarning("[LocalizedText] LocalizationSystem не найдена!");
            return;
        }
        
        if (string.IsNullOrEmpty(localizationKey))
        {
            Debug.LogWarning("[LocalizedText] Ключ локализации не задан!");
            return;
        }
        
        if (textComponent != null)
        {
            textComponent.text = LocalizationSystem.Instance.GetText(localizationKey);
        }
    }
    
    /// <summary>
    /// Обновить текст с форматированием
    /// </summary>
    public void UpdateText(params object[] args)
    {
        if (LocalizationSystem.Instance == null || textComponent == null)
            return;
        
        textComponent.text = LocalizationSystem.Instance.GetText(localizationKey, args);
    }
}
