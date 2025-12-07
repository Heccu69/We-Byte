using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Менеджер слотов сохранений в главном меню
/// </summary>
public class SaveSlotsManager : MonoBehaviour
{
    [Header("UI Слотов")]
    public GameObject[] slotPanels; // Панели слотов (3 штуки)
    public TextMeshProUGUI[] slotNames; // Названия сохранений
    public TextMeshProUGUI[] slotInfoTexts; // Информация (дата, уровень, счет)
    public Button[] playButtons; // Кнопки "Играть"
    public Button[] deleteButtons; // Кнопки "Удалить"
    
    [Header("UI Создания сохранения")]
    public GameObject createSavePanel; // Панель создания нового сохранения
    public TMP_InputField saveNameInput; // Поле ввода названия
    public Button createButton; // Кнопка создать
    public Button cancelButton; // Кнопка отмены
    
    [Header("Настройки")]
    public string gameSceneName = "GameScene"; // Название сцены игры
    
    private int selectedSlotIndex = -1;
    
    void Start()
    {
        // Скрываем панель создания
        if (createSavePanel != null)
            createSavePanel.SetActive(false);
        
        // Обновляем отображение слотов
        RefreshSlots();
        
        // Привязываем кнопки
        SetupButtons();
    }
    
    /// <summary>
    /// Настройка кнопок слотов
    /// </summary>
    void SetupButtons()
    {
        for (int i = 0; i < playButtons.Length; i++)
        {
            int slotIndex = i; // Локальная копия для замыкания
            
            if (playButtons[i] != null)
            {
                playButtons[i].onClick.AddListener(() => LoadGame(slotIndex));
            }
            
            if (deleteButtons[i] != null)
            {
                deleteButtons[i].onClick.AddListener(() => DeleteSave(slotIndex));
            }
        }
        
        // Кнопки панели создания
        if (createButton != null)
            createButton.onClick.AddListener(ConfirmCreateSave);
        
        if (cancelButton != null)
            cancelButton.onClick.AddListener(CancelCreateSave);
    }
    
    /// <summary>
    /// Обновить отображение всех слотов
    /// </summary>
    public void RefreshSlots()
    {
        SaveData[] slots = SaveSystem.GetAllSlots();
        
        for (int i = 0; i < slots.Length && i < slotPanels.Length; i++)
        {
            UpdateSlotUI(i, slots[i]);
        }
    }
    
    /// <summary>
    /// Обновить UI конкретного слота
    /// </summary>
    void UpdateSlotUI(int slotIndex, SaveData saveData)
    {
        if (slotIndex >= slotPanels.Length) return;
        
        bool slotExists = saveData != null;
        
        // Название
        if (slotNames[slotIndex] != null)
        {
            slotNames[slotIndex].text = slotExists ? saveData.slotName : "Пустой слот";
        }
        
        // Информация
        if (slotInfoTexts[slotIndex] != null)
        {
            if (slotExists)
            {
                string info = $"Уровень: {saveData.currentLevel}\n";
                info += $"Счет: {saveData.currentScore}\n";
                info += $"Сохранено: {saveData.saveDateTime}";
                slotInfoTexts[slotIndex].text = info;
            }
            else
            {
                slotInfoTexts[slotIndex].text = "Нажмите для создания";
            }
        }
        
        // Кнопки
        if (playButtons[slotIndex] != null)
        {
            playButtons[slotIndex].interactable = slotExists;
            TextMeshProUGUI buttonText = playButtons[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = slotExists ? "Играть" : "Создать";
            }
        }
        
        if (deleteButtons[slotIndex] != null)
        {
            deleteButtons[slotIndex].gameObject.SetActive(slotExists);
        }
    }
    
    /// <summary>
    /// Загрузить игру из слота
    /// </summary>
    public void LoadGame(int slotIndex)
    {
        if (SaveSystem.SlotExists(slotIndex))
        {
            // Загружаем сохранение
            SaveData saveData = SaveSystem.LoadFromSlot(slotIndex);
            
            // Сохраняем индекс текущего слота в PlayerPrefs
            PlayerPrefs.SetInt("CurrentSaveSlot", slotIndex);
            PlayerPrefs.Save();
            
            Debug.Log($"Загружаем игру из слота {slotIndex}: {saveData.slotName}");
            
            // Загружаем сцену игры
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            // Слот пуст - показываем панель создания
            ShowCreateSavePanel(slotIndex);
        }
    }
    
    /// <summary>
    /// Показать панель создания нового сохранения
    /// </summary>
    void ShowCreateSavePanel(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        
        if (createSavePanel != null)
        {
            createSavePanel.SetActive(true);
            
            // Очищаем поле ввода
            if (saveNameInput != null)
            {
                saveNameInput.text = $"Сохранение {slotIndex + 1}";
                saveNameInput.Select();
                saveNameInput.ActivateInputField();
            }
        }
    }
    
    /// <summary>
    /// Подтвердить создание сохранения
    /// </summary>
    void ConfirmCreateSave()
    {
        if (selectedSlotIndex < 0) return;
        
        string saveName = saveNameInput != null && !string.IsNullOrEmpty(saveNameInput.text) 
            ? saveNameInput.text 
            : $"Сохранение {selectedSlotIndex + 1}";
        
        // Создаем новое сохранение
        SaveData newSave = new SaveData();
        newSave.slotName = saveName;
        SaveSystem.SaveToSlot(newSave, selectedSlotIndex);
        
        // Скрываем панель
        if (createSavePanel != null)
            createSavePanel.SetActive(false);
        
        // Обновляем UI
        RefreshSlots();
        
        Debug.Log($"Создано новое сохранение: {saveName} в слоте {selectedSlotIndex}");
        
        // Автоматически загружаем игру
        LoadGame(selectedSlotIndex);
    }
    
    /// <summary>
    /// Отменить создание сохранения
    /// </summary>
    void CancelCreateSave()
    {
        if (createSavePanel != null)
            createSavePanel.SetActive(false);
        
        selectedSlotIndex = -1;
    }
    
    /// <summary>
    /// Удалить сохранение
    /// </summary>
    public void DeleteSave(int slotIndex)
    {
        // Можно добавить подтверждение удаления
        SaveSystem.DeleteSlot(slotIndex);
        RefreshSlots();
        Debug.Log($"Удалено сохранение из слота {slotIndex}");
    }
}
