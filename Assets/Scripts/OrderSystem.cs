using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Система управления заказами
/// </summary>
public class OrderSystem : MonoBehaviour
{
    public static OrderSystem Instance { get; private set; }
    
    [Header("Настройки заказов")]
    public int minKorzhCount = 2; // Минимальное количество коржей в заказе
    public int maxKorzhCount = 5; // Максимальное количество коржей в заказе
    
    private int currentOrderKorzhCount = 0;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        GenerateNewOrder();
    }
    
    /// <summary>
    /// Генерировать новый заказ
    /// </summary>
    public void GenerateNewOrder()
    {
        currentOrderKorzhCount = Random.Range(minKorzhCount, maxKorzhCount + 1);
        Debug.Log($"Новый заказ: {currentOrderKorzhCount} коржей");
        
        // Уведомляем OrderDisplay об обновлении
        OrderDisplay display = FindObjectOfType<OrderDisplay>();
        if (display != null)
        {
            display.UpdateOrderDisplay();
        }
    }
    
    /// <summary>
    /// Получить текущее количество коржей в заказе
    /// </summary>
    public int GetCurrentOrderKorzhCount()
    {
        return currentOrderKorzhCount;
    }
    
    /// <summary>
    /// Проверить выполнение заказа
    /// </summary>
    public bool CheckOrder(int korzhCount)
    {
        return korzhCount == currentOrderKorzhCount;
    }
    
    /// <summary>
    /// Заказ выполнен ПРАВИЛЬНО
    /// </summary>
    public void CompleteOrderCorrect()
    {
        Debug.Log($"✅ Заказ выполнен ПРАВИЛЬНО! {currentOrderKorzhCount} коржей");
        
        // Добавляем очко
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
        }
        
        // Сохраняем статистику заказов
        SaveOrderStats(true);
        
        // Генерируем новый заказ
        GenerateNewOrder();
    }
    
    /// <summary>
    /// Заказ выполнен НЕПРАВИЛЬНО
    /// </summary>
    public void CompleteOrderIncorrect(int actualKorzhCount)
    {
        Debug.Log($"❌ Заказ выполнен НЕПРАВИЛЬНО! Нужно: {currentOrderKorzhCount}, Дано: {actualKorzhCount}");
        
        // НЕ добавляем очки за неправильный заказ
        
        // Сохраняем статистику заказов
        SaveOrderStats(false);
        
        // Генерируем новый заказ
        GenerateNewOrder();
    }
    
    /// <summary>
    /// Сохранить статистику заказов
    /// </summary>
    private void SaveOrderStats(bool isCorrect)
    {
        SaveData saveData = SaveSystem.Load();
        saveData.totalCompletedOrders++;
        if (isCorrect)
        {
            saveData.totalCorrectOrders++;
        }
        SaveSystem.Save(saveData);
    }
}
