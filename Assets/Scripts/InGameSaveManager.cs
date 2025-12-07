using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Менеджер сохранений во время игры
/// </summary>
public class InGameSaveManager : MonoBehaviour
{
    public static InGameSaveManager Instance { get; private set; }
    
    [Header("Настройки")]
    public KeyCode saveKey = KeyCode.F5; // Клавиша быстрого сохранения
    public bool autoSaveEnabled = true; // Автосохранение
    public float autoSaveInterval = 300f; // Интервал автосохранения (5 минут)
    
    [Header("UI")]
    public TextMeshProUGUI saveNotificationText; // Текст уведомления о сохранении
    public float notificationDuration = 2f; // Длительность показа уведомления
    
    private int currentSlotIndex = 0;
    private float playTime = 0f;
    private float autoSaveTimer = 0f;
    private SaveData currentSaveData;
    
    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Загружаем текущий слот
        currentSlotIndex = PlayerPrefs.GetInt("CurrentSaveSlot", 0);
        LoadCurrentSave();
    }
    
    void Start()
    {
        // Скрываем уведомление
        if (saveNotificationText != null)
            saveNotificationText.gameObject.SetActive(false);
    }
    
    void Update()
    {
        // Увеличиваем время игры
        playTime += Time.deltaTime;
        
        // Быстрое сохранение по нажатию клавиши
        if (Input.GetKeyDown(saveKey))
        {
            QuickSave();
        }
        
        // Автосохранение
        if (autoSaveEnabled)
        {
            autoSaveTimer += Time.deltaTime;
            if (autoSaveTimer >= autoSaveInterval)
            {
                AutoSave();
                autoSaveTimer = 0f;
            }
        }
    }
    
    /// <summary>
    /// Загрузить текущее сохранение
    /// </summary>
    void LoadCurrentSave()
    {
        currentSaveData = SaveSystem.LoadFromSlot(currentSlotIndex);
        
        if (currentSaveData == null)
        {
            Debug.LogWarning($"Сохранение в слоте {currentSlotIndex} не найдено. Создаем новое.");
            currentSaveData = new SaveData();
            currentSaveData.slotIndex = currentSlotIndex;
        }
        
        // Применяем загруженные данные
        ApplySaveData();
        
        Debug.Log($"Загружено сохранение: {currentSaveData.slotName}");
    }
    
    /// <summary>
    /// Применить данные из сохранения к игре
    /// </summary>
    void ApplySaveData()
    {
        // Восстанавливаем время игры
        playTime = currentSaveData.playTime;
        
        // Применяем счет
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetScore(currentSaveData.currentScore);
            ScoreManager.Instance.SetHighScore(currentSaveData.highScore);
        }
        
        // Можно добавить восстановление других параметров
    }
    
    /// <summary>
    /// Собрать текущие данные игры для сохранения
    /// </summary>
    SaveData CollectCurrentGameData()
    {
        // Обновляем время игры
        currentSaveData.playTime = playTime;
        
        // Собираем счет
        if (ScoreManager.Instance != null)
        {
            currentSaveData.currentScore = ScoreManager.Instance.GetScore();
            currentSaveData.highScore = ScoreManager.Instance.GetHighScore();
        }
        
        // Собираем статистику заказов
        // (уже сохраняется автоматически в OrderSystem)
        
        return currentSaveData;
    }
    
    /// <summary>
    /// Быстрое сохранение (F5)
    /// </summary>
    public void QuickSave()
    {
        SaveData dataToSave = CollectCurrentGameData();
        SaveSystem.SaveToSlot(dataToSave, currentSlotIndex);
        
        ShowSaveNotification($"Игра сохранена ({dataToSave.slotName})");
        Debug.Log($"Быстрое сохранение в слот {currentSlotIndex}");
    }
    
    /// <summary>
    /// Автосохранение
    /// </summary>
    void AutoSave()
    {
        SaveData dataToSave = CollectCurrentGameData();
        SaveSystem.SaveToSlot(dataToSave, currentSlotIndex);
        
        ShowSaveNotification("Автосохранение выполнено");
        Debug.Log($"Автосохранение в слот {currentSlotIndex}");
    }
    
    /// <summary>
    /// Сохранить в конкретный слот
    /// </summary>
    public void SaveToSlot(int slotIndex, string saveName = null)
    {
        SaveData dataToSave = CollectCurrentGameData();
        
        if (!string.IsNullOrEmpty(saveName))
        {
            dataToSave.slotName = saveName;
        }
        
        SaveSystem.SaveToSlot(dataToSave, slotIndex);
        ShowSaveNotification($"Сохранено: {dataToSave.slotName}");
        Debug.Log($"Сохранено в слот {slotIndex}: {dataToSave.slotName}");
    }
    
    /// <summary>
    /// Показать уведомление о сохранении
    /// </summary>
    void ShowSaveNotification(string message)
    {
        if (saveNotificationText != null)
        {
            saveNotificationText.text = message;
            StopAllCoroutines();
            StartCoroutine(ShowNotificationCoroutine());
        }
    }
    
    /// <summary>
    /// Корутина показа уведомления
    /// </summary>
    IEnumerator ShowNotificationCoroutine()
    {
        saveNotificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(notificationDuration);
        saveNotificationText.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Получить текущий индекс слота
    /// </summary>
    public int GetCurrentSlotIndex()
    {
        return currentSlotIndex;
    }
    
    /// <summary>
    /// Получить текущие данные сохранения
    /// </summary>
    public SaveData GetCurrentSaveData()
    {
        return currentSaveData;
    }
}
