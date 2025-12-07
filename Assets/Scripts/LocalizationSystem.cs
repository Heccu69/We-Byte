using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Система локализации для поддержки нескольких языков
/// </summary>
public class LocalizationSystem : MonoBehaviour
{
    public static LocalizationSystem Instance { get; private set; }
    
    [Header("Настройки")]
    public string currentLanguage = "ru"; // Текущий язык (ru/en)
    
    // Словарь переводов
    private Dictionary<string, Dictionary<string, string>> translations;
    
    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Инициализируем переводы
        InitializeTranslations();
        
        // Загружаем сохраненный язык
        LoadLanguage();
    }
    
    /// <summary>
    /// Инициализация всех переводов
    /// </summary>
    void InitializeTranslations()
    {
        translations = new Dictionary<string, Dictionary<string, string>>();
        
        // Главное меню
        AddTranslation("menu_play", "ru", "Играть");
        AddTranslation("menu_play", "en", "Play");
        
        AddTranslation("menu_continue", "ru", "Продолжить");
        AddTranslation("menu_continue", "en", "Continue");
        
        AddTranslation("menu_settings", "ru", "Настройки");
        AddTranslation("menu_settings", "en", "Settings");
        
        AddTranslation("menu_exit", "ru", "Выход");
        AddTranslation("menu_exit", "en", "Exit");
        
        AddTranslation("menu_back", "ru", "Назад");
        AddTranslation("menu_back", "en", "Back");
        
        // Настройки
        AddTranslation("settings_title", "ru", "Настройки");
        AddTranslation("settings_title", "en", "Settings");
        
        AddTranslation("settings_language", "ru", "Язык");
        AddTranslation("settings_language", "en", "Language");
        
        AddTranslation("settings_russian", "ru", "Русский");
        AddTranslation("settings_russian", "en", "Russian");
        
        AddTranslation("settings_english", "ru", "Английский");
        AddTranslation("settings_english", "en", "English");
        
        // Игра
        AddTranslation("game_score", "ru", "Очки: {0}");
        AddTranslation("game_score", "en", "Score: {0}");
        
        AddTranslation("game_highscore", "ru", "Рекорд: {0}");
        AddTranslation("game_highscore", "en", "High Score: {0}");
        
        AddTranslation("game_level", "ru", "Уровень {0}");
        AddTranslation("game_level", "en", "Level {0}");
        
        AddTranslation("game_pause", "ru", "Пауза");
        AddTranslation("game_pause", "en", "Pause");
        
        AddTranslation("game_resume", "ru", "Продолжить");
        AddTranslation("game_resume", "en", "Resume");
        
        AddTranslation("game_exit_to_menu", "ru", "Выйти в меню");
        AddTranslation("game_exit_to_menu", "en", "Exit to Menu");
        
        // Сохранения
        AddTranslation("save_saved", "ru", "Игра сохранена");
        AddTranslation("save_saved", "en", "Game Saved");
        
        AddTranslation("save_autosave", "ru", "Автосохранение");
        AddTranslation("save_autosave", "en", "Autosave");
        
        AddTranslation("save_quicksave", "ru", "Быстрое сохранение");
        AddTranslation("save_quicksave", "en", "Quick Save");
        
        AddTranslation("save_new", "ru", "Новое сохранение");
        AddTranslation("save_new", "en", "New Save");
        
        // Заказы
        AddTranslation("order_correct", "ru", "Правильно!");
        AddTranslation("order_correct", "en", "Correct!");
        
        AddTranslation("order_wrong", "ru", "Неправильно!");
        AddTranslation("order_wrong", "en", "Wrong!");
        
        AddTranslation("order_completed", "ru", "Заказ выполнен");
        AddTranslation("order_completed", "en", "Order Completed");
        
        // Враги
        AddTranslation("enemy_defeated", "ru", "Враг побежден");
        AddTranslation("enemy_defeated", "en", "Enemy Defeated");
        
        AddTranslation("enemy_escaped", "ru", "Враг сбежал");
        AddTranslation("enemy_escaped", "en", "Enemy Escaped");
        
        Debug.Log($"[LocalizationSystem] Загружено {translations.Count} переводов");
    }
    
    /// <summary>
    /// Добавить перевод
    /// </summary>
    void AddTranslation(string key, string language, string value)
    {
        if (!translations.ContainsKey(key))
        {
            translations[key] = new Dictionary<string, string>();
        }
        
        translations[key][language] = value;
    }
    
    /// <summary>
    /// Получить перевод по ключу
    /// </summary>
    public string GetText(string key)
    {
        if (translations.ContainsKey(key) && translations[key].ContainsKey(currentLanguage))
        {
            return translations[key][currentLanguage];
        }
        
        Debug.LogWarning($"[LocalizationSystem] Перевод не найден: {key} ({currentLanguage})");
        return key; // Возвращаем ключ, если перевод не найден
    }
    
    /// <summary>
    /// Получить перевод с форматированием
    /// </summary>
    public string GetText(string key, params object[] args)
    {
        string text = GetText(key);
        return string.Format(text, args);
    }
    
    /// <summary>
    /// Изменить язык
    /// </summary>
    public void SetLanguage(string language)
    {
        if (language != "ru" && language != "en")
        {
            Debug.LogWarning($"[LocalizationSystem] Неизвестный язык: {language}");
            return;
        }
        
        currentLanguage = language;
        SaveLanguage();
        
        // Уведомляем все LocalizedText компоненты об изменении языка
        LocalizedText[] localizedTexts = FindObjectsOfType<LocalizedText>();
        foreach (LocalizedText text in localizedTexts)
        {
            text.UpdateText();
        }
        
        Debug.Log($"[LocalizationSystem] Язык изменен на: {language}");
    }
    
    /// <summary>
    /// Сохранить выбранный язык
    /// </summary>
    void SaveLanguage()
    {
        PlayerPrefs.SetString("Language", currentLanguage);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Загрузить сохраненный язык
    /// </summary>
    void LoadLanguage()
    {
        currentLanguage = PlayerPrefs.GetString("Language", "ru");
        Debug.Log($"[LocalizationSystem] Загружен язык: {currentLanguage}");
    }
    
    /// <summary>
    /// Получить текущий язык
    /// </summary>
    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }
}
