using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Отображение заказа на PC
/// </summary>
public class OrderDisplay : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI orderText; // TextMeshPro текст для отображения заказа
    public GameObject orderPanel; // Панель с заказом (опционально)
    
    void Start()
    {
        UpdateOrderDisplay();
    }
    
    /// <summary>
    /// Обновить отображение заказа
    /// </summary>
    public void UpdateOrderDisplay()
    {
        if (OrderSystem.Instance != null && orderText != null)
        {
            int korzhCount = OrderSystem.Instance.GetCurrentOrderKorzhCount();
            orderText.text = $"ЗАКАЗ:\n{korzhCount} коржей";
        }
    }
}
