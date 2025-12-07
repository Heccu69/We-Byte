using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Данные одного клиента
/// </summary>
[System.Serializable]
public class CustomerData
{
    public Sprite customerSprite; // Спрайт клиента
    [TextArea(2, 4)]
    public string customerDialogue; // Реплика клиента
}

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
    public TextMeshProUGUI customerDialogueText; // Текст реплики клиента
    public CustomerData[] customers; // Массив клиентов в порядке очереди
    
    private int currentCustomerIndex = 0; // Индекс текущего клиента
    
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
        
        // Показываем следующего клиента в очереди
        ShowNextCustomer();
    }
    
    /// <summary>
    /// Показать следующего клиента в очереди
    /// </summary>
    void ShowNextCustomer()
    {
        if (customers == null || customers.Length == 0)
        {
            Debug.LogWarning("Массив клиентов пуст!");
            return;
        }
        
        // Получаем текущего клиента
        CustomerData currentCustomer = customers[currentCustomerIndex];
        
        // Обновляем спрайт
        if (customerImage != null && currentCustomer.customerSprite != null)
        {
            customerImage.sprite = currentCustomer.customerSprite;
            customerImage.enabled = true;
        }
        
        // Обновляем реплику
        if (customerDialogueText != null)
        {
            customerDialogueText.text = currentCustomer.customerDialogue;
        }
        
        // Переходим к следующему клиенту (циклически)
        currentCustomerIndex = (currentCustomerIndex + 1) % customers.Length;
        
        Debug.Log($"👤 Клиент говорит: {currentCustomer.customerDialogue}");
    }
}
