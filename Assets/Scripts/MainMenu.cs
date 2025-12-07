using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управление главным меню игры
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("UI Панели")]
    public GameObject menuPanel; // Главная панель меню
    public GameObject settingsPanel; // Панель настроек
    
    void Start()
    {
        // При запуске показываем только главное меню
        ShowMainMenu();
    }
    
    /// <summary>
    /// Показать главное меню
    /// </summary>
    public void ShowMainMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }
    
    /// <summary>
    /// Кнопка "Играть" - загружает игровую сцену
    /// </summary>
    public void OnPlayButton()
    {
        Debug.Log("🎮 Начинаем игру!");
        // Загружаем игровую сцену (замените "GameScene" на имя вашей игровой сцены)
        SceneManager.LoadScene("GameScene");
    }
    
    /// <summary>
    /// Кнопка "Продолжить" (SavesButton) - загружает последнее сохранение и запускает игру
    /// </summary>
    public void OnSavesButton()
    {
        Debug.Log("🎮 Продолжаем игру с последнего сохранения");
        
        // Находим последнее сохранение
        int slotIndex;
        SaveData lastSave = SaveSystem.GetLastSave(out slotIndex);
        
        if (lastSave != null)
        {
            // Сохраняем индекс слота для загрузки
            PlayerPrefs.SetInt("CurrentSaveSlot", slotIndex);
            PlayerPrefs.Save();
            
            Debug.Log($"Загружаем последнее сохранение из слота {slotIndex}");
            
            // Загружаем игровую сцену
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.LogWarning("⚠️ Нет сохранений! Начинаем новую игру.");
            
            // Если нет сохранений, создаем новое и начинаем игру
            int newSlotIndex = SaveSystem.CreateNewSave("Новое сохранение");
            
            if (newSlotIndex >= 0)
            {
                PlayerPrefs.SetInt("CurrentSaveSlot", newSlotIndex);
                PlayerPrefs.Save();
            }
            
            SceneManager.LoadScene("GameScene");
        }
    }
    
    /// <summary>
    /// Кнопка "Настройки" - открывает панель настроек
    /// </summary>
    public void OnSettingsButton()
    {
        Debug.Log("⚙️ Открываем настройки");
        if (menuPanel != null) menuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
    
    /// <summary>
    /// Кнопка "Назад" - возврат в главное меню
    /// </summary>
    public void OnBackButton()
    {
        ShowMainMenu();
    }
    
    /// <summary>
    /// Кнопка "Выход" - выход из игры
    /// </summary>
    public void OnExitButton()
    {
        Debug.Log("👋 Выход из игры");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
