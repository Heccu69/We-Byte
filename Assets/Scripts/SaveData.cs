using UnityEngine;
using System;

[System.Serializable]
public class SaveData
{
    // Метаданные слота
    public string slotName = "Новое сохранение";  // Название слота
    public string saveDateTime = "";               // Дата и время сохранения
    public int slotIndex = 0;                      // Индекс слота (0-2)
    
    // Настройки
    public string selectedLanguage = "en"; // Язык по умолчанию
    
    // Прогресс игры
    public int currentLevel = 1;          // Текущий уровень
    public int currentScore = 0;          // Текущие очки в сессии
    public int highScore = 0;             // Лучший счет
    public int totalCompletedOrders = 0;  // Всего выполнено заказов
    public int totalCorrectOrders = 0;    // Всего правильных заказов
    public float playTime = 0f;           // Время игры в секундах
    
    /// <summary>
    /// Обновить время сохранения
    /// </summary>
    public void UpdateSaveTime()
    {
        saveDateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
    }
}

