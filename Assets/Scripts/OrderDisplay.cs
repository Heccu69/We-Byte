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
    
    [Header("Клиенты")]
    public UnityEngine.UI.Image customerImage; // Image для отображения спрайта клиента
    public Sprite[] customerSprites; // Массив спрайтов клиентов
    
    void Start()
    {
        UpdateOrderDisplay();
    }
    
    /// <summary>
    /// Обновить отображение заказа
    /// </summary>
    public void UpdateOrderDisplay()
    {
        // Обновляем текст заказа
        if (OrderSystem.Instance != null && orderText != null)
        {
            int korzhCount = OrderSystem.Instance.GetCurrentOrderKorzhCount();
            orderText.text = $"ЗАКАЗ:\n{korzhCount} коржей";
        }
        
        // Обновляем спрайт клиента
        UpdateCustomerSprite();
    }
    
    /// <summary>
    /// Выбрать случайного клиента
    /// </summary>
    void UpdateCustomerSprite()
    {
        if (customerImage != null && customerSprites != null && customerSprites.Length > 0)
        {
            // Выбираем случайный спрайт
            Sprite randomCustomer = customerSprites[Random.Range(0, customerSprites.Length)];
            customerImage.sprite = randomCustomer;
            
            // Включаем отображение клиента
            customerImage.enabled = true;
        }
    }
}
