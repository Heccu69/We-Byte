using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Компонент для выбора языка в настройках
/// </summary>
public class LanguageSelector : MonoBehaviour
{
    [Header("Кнопки Языков")]
    public Button russianButton; // Кнопка "Русский"
    public Button englishButton; // Кнопка "English"
    
    [Header("Визуальная Индикация")]
    public Color selectedColor = new Color(0.3f, 0.8f, 0.3f); // Зеленый для выбранного
    public Color normalColor = Color.white; // Белый для невыбранного
    
    void Start()
    {
        // Привязываем кнопки
        if (russianButton != null)
            russianButton.onClick.AddListener(() => SetLanguage("ru"));
        
        if (englishButton != null)
            englishButton.onClick.AddListener(() => SetLanguage("en"));
        
        // Обновляем визуальное состояние кнопок
        UpdateButtonStates();
    }
    
    /// <summary>
    /// Установить язык
    /// </summary>
    public void SetLanguage(string language)
    {
        if (LocalizationSystem.Instance != null)
        {
            LocalizationSystem.Instance.SetLanguage(language);
            UpdateButtonStates();
            
            Debug.Log($"[LanguageSelector] Язык изменен на: {language}");
        }
        else
        {
            Debug.LogError("[LanguageSelector] LocalizationSystem не найдена!");
        }
    }
    
    /// <summary>
    /// Обновить визуальное состояние кнопок
    /// </summary>
    void UpdateButtonStates()
    {
        if (LocalizationSystem.Instance == null)
            return;
        
        string currentLanguage = LocalizationSystem.Instance.GetCurrentLanguage();
        
        // Обновляем цвет русской кнопки
        if (russianButton != null)
        {
            ColorBlock colors = russianButton.colors;
            colors.normalColor = (currentLanguage == "ru") ? selectedColor : normalColor;
            russianButton.colors = colors;
        }
        
        // Обновляем цвет английской кнопки
        if (englishButton != null)
        {
            ColorBlock colors = englishButton.colors;
            colors.normalColor = (currentLanguage == "en") ? selectedColor : normalColor;
            englishButton.colors = colors;
        }
    }
    
    /// <summary>
    /// Кнопка "Русский"
    /// </summary>
    public void OnRussianButton()
    {
        SetLanguage("ru");
    }
    
    /// <summary>
    /// Кнопка "English"
    /// </summary>
    public void OnEnglishButton()
    {
        SetLanguage("en");
    }
}
