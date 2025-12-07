using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SaveData saveData;

    void Start()
    {
        // Загружаем данные при старте
        saveData = SaveSystem.Load();
        
        // Применяем сохранённые настройки
        ApplySettings();
    }

    // Метод для смены языка
    public void ChangeLanguage(string langCode)
    {
        saveData.selectedLanguage = langCode;
        SaveSystem.Save(saveData);
        ApplyLanguage(langCode);
    }

    // Метод для обновления уровня
    public void CompleteLevel(int level)
    {
        saveData.currentLevel = level;
        SaveSystem.Save(saveData);
    }

    // Применяем настройки (пример)
    private void ApplySettings()
    {
        Debug.Log("Язык: " + saveData.selectedLanguage);
        Debug.Log("Уровень: " + saveData.currentLevel);
        // Здесь добавьте код для применения громкости и др.
    }

    private void ApplyLanguage(string langCode)
    {
        Debug.Log("Установлен язык: " + langCode);
        // Здесь код для переключения текста в UI
    }
}
