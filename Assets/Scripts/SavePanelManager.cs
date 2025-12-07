using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

/// <summary>
/// Менеджер панели сохранений в меню
/// Показывает информацию о последнем сохранении и позволяет быстро загрузить его
/// </summary>
public class SavePanelManager : MonoBehaviour
{
    [Header("UI Элементы")]
    public TextMeshProUGUI lastSaveTimeText; // Текст с временем последнего сохранения
    public TextMeshProUGUI lastSaveInfoText; // Дополнительная информация о сохранении
    public Button loadLastSaveButton; // Кнопка загрузки последнего сохранения
    public Button viewAllSavesButton; // Кнопка просмотра всех сохранений
    public Button backButton; // Кнопка назад
    
    [Header("Панели")]
    public GameObject quickSavePanel; // Панель быстрого доступа
    public GameObject allSavesPanel; // Панель всех сохранений (SaveSlotsManager)
    
    [Header("Настройки")]
    public string gameSceneName = "GameScene"; // Название игровой сцены
    
    private SaveData lastSaveData;
    private int lastSaveSlotIndex = -1;
    
    void OnEnable()
    {
        // Обновляем информацию при открытии панели
        RefreshLastSaveInfo();
        
        // Показываем панель быстрого доступа
        ShowQuickSavePanel();
    }
    
    void Start()
    {
        // Привязываем кнопки
        if (loadLastSaveButton != null)
            loadLastSaveButton.onClick.AddListener(LoadLastSave);
        
        if (viewAllSavesButton != null)
            viewAllSavesButton.onClick.AddListener(ShowAllSaves);
        
        if (backButton != null)
            backButton.onClick.AddListener(OnBackButton);
    }
    
    /// <summary>
    /// Обновить информацию о последнем сохранении
    /// </summary>
    public void RefreshLastSaveInfo()
    {
        // Получаем все сохранения
        SaveData[] allSaves = SaveSystem.GetAllSlots();
        
        // Находим последнее сохранение (с самой поздней датой)
        lastSaveData = null;
        lastSaveSlotIndex = -1;
        
        for (int i = 0; i < allSaves.Length; i++)
        {
            if (allSaves[i] != null)
            {
                if (lastSaveData == null || IsNewerSave(allSaves[i], lastSaveData))
                {
                    lastSaveData = allSaves[i];
                    lastSaveSlotIndex = i;
                }
            }
        }
        
        // Обновляем UI
        UpdateUI();
    }
    
    /// <summary>
    /// Проверить, является ли сохранение более новым
    /// </summary>
    bool IsNewerSave(SaveData save1, SaveData save2)
    {
        if (string.IsNullOrEmpty(save1.saveDateTime))
            return false;
        if (string.IsNullOrEmpty(save2.saveDateTime))
            return true;
        
        // Сравниваем строки дат (формат: dd.MM.yyyy HH:mm)
        // Для более точного сравнения можно использовать DateTime.Parse
        try
        {
            System.DateTime date1 = System.DateTime.ParseExact(save1.saveDateTime, "dd.MM.yyyy HH:mm", null);
            System.DateTime date2 = System.DateTime.ParseExact(save2.saveDateTime, "dd.MM.yyyy HH:mm", null);
            return date1 > date2;
        }
        catch
        {
            // Если не удалось распарсить, сравниваем как строки
            return string.Compare(save1.saveDateTime, save2.saveDateTime) > 0;
        }
    }
    
    /// <summary>
    /// Обновить UI с информацией о последнем сохранении
    /// </summary>
    void UpdateUI()
    {
        if (lastSaveData != null)
        {
            // Показываем время последнего сохранения
            if (lastSaveTimeText != null)
            {
                lastSaveTimeText.text = $"Последнее сохранение:\n{lastSaveData.saveDateTime}";
            }
            
            // Показываем дополнительную информацию
            if (lastSaveInfoText != null)
            {
                string info = $"<b>{lastSaveData.slotName}</b>\n";
                info += $"Уровень: {lastSaveData.currentLevel}\n";
                info += $"Счет: {lastSaveData.currentScore}\n";
                info += $"Лучший счет: {lastSaveData.highScore}";
                lastSaveInfoText.text = info;
            }
            
            // Активируем кнопку загрузки
            if (loadLastSaveButton != null)
            {
                loadLastSaveButton.interactable = true;
            }
        }
        else
        {
            // Нет сохранений
            if (lastSaveTimeText != null)
            {
                lastSaveTimeText.text = "Нет сохранений";
            }
            
            if (lastSaveInfoText != null)
            {
                lastSaveInfoText.text = "Создайте новое сохранение\nчтобы начать игру";
            }
            
            // Деактивируем кнопку загрузки
            if (loadLastSaveButton != null)
            {
                loadLastSaveButton.interactable = false;
            }
        }
    }
    
    /// <summary>
    /// Загрузить последнее сохранение
    /// </summary>
    public void LoadLastSave()
    {
        if (lastSaveData != null && lastSaveSlotIndex >= 0)
        {
            // Сохраняем индекс текущего слота
            PlayerPrefs.SetInt("CurrentSaveSlot", lastSaveSlotIndex);
            PlayerPrefs.Save();
            
            Debug.Log($"Загружаем последнее сохранение из слота {lastSaveSlotIndex}: {lastSaveData.slotName}");
            
            // Загружаем игровую сцену
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("Нет доступных сохранений для загрузки");
        }
    }
    
    /// <summary>
    /// Показать панель всех сохранений
    /// </summary>
    public void ShowAllSaves()
    {
        if (quickSavePanel != null)
            quickSavePanel.SetActive(false);
        
        if (allSavesPanel != null)
        {
            allSavesPanel.SetActive(true);
            
            // Обновляем слоты в SaveSlotsManager
            SaveSlotsManager slotsManager = allSavesPanel.GetComponent<SaveSlotsManager>();
            if (slotsManager != null)
            {
                slotsManager.RefreshSlots();
            }
        }
    }
    
    /// <summary>
    /// Показать панель быстрого доступа
    /// </summary>
    public void ShowQuickSavePanel()
    {
        if (quickSavePanel != null)
            quickSavePanel.SetActive(true);
        
        if (allSavesPanel != null)
            allSavesPanel.SetActive(false);
    }
    
    /// <summary>
    /// Кнопка "Назад"
    /// </summary>
    void OnBackButton()
    {
        // Если открыта панель всех сохранений, возвращаемся к быстрому доступу
        if (allSavesPanel != null && allSavesPanel.activeSelf)
        {
            ShowQuickSavePanel();
        }
        else
        {
            // Иначе закрываем панель сохранений
            gameObject.SetActive(false);
            
            // Показываем главное меню
            MainMenu mainMenu = FindObjectOfType<MainMenu>();
            if (mainMenu != null)
            {
                mainMenu.ShowMainMenu();
            }
        }
    }
}
