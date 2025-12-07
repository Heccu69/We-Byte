using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

<<<<<<< HEAD
public class MainMenu : MonoBehaviour
{
    public void StartDay1()
    {
        SceneManager.LoadScene("Day1");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void Saves()
    {
        SceneManager.LoadScene("Saves");
=======
/// <summary>
/// Управление главным меню игры
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("UI Панели")]
    public GameObject menuPanel; // Главная панель меню
    public GameObject savesPanel; // Панель сохранений
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
        if (savesPanel != null) savesPanel.SetActive(false);
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
    /// Кнопка "Сохранения" - открывает панель сохранений
    /// </summary>
    public void OnSavesButton()
    {
        Debug.Log("💾 Открываем сохранения");
        if (menuPanel != null) menuPanel.SetActive(false);
        if (savesPanel != null) savesPanel.SetActive(true);
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
>>>>>>> origin/sofa
    }
}
