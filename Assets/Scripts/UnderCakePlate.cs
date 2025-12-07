using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Тарелка для сборки торта - подсчитывает коржи в стопке
/// </summary>
public class UnderCakePlate : MonoBehaviour
{
    [Header("Настройки")]
    public float checkRadius = 2f; // Радиус проверки коржей над тарелкой
    public float stackTolerance = 0.3f; // Допуск для определения стопки (расстояние между коржами)
    public LayerMask korzhLayer; // Слой коржей (опционально)
    
    [Header("Отладка")]
    public bool showDebugInfo = true;
    
    private List<GameObject> korzhsOnPlate = new List<GameObject>();
    
    // Убрали автоматическую проверку - теперь только по кнопке
    
    /// <summary>
    /// НАЖАТИЕ КНОПКИ - проверить заказ
    /// Привязать этот метод к UI кнопке!
    /// </summary>
    public void CheckOrderButton()
    {
        // Очищаем список
        korzhsOnPlate.Clear();
        
        // Находим все коржи в радиусе над тарелкой
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);
        
        foreach (Collider2D col in colliders)
        {
            // Проверяем что это корж
            PickupObject pickupObj = col.GetComponent<PickupObject>();
            if (pickupObj != null && pickupObj.objectType == ObjectType.Korzh)
            {
                // Проверяем что корж НЕ в руках
                if (!pickupObj.isPickedUp)
                {
                    korzhsOnPlate.Add(col.gameObject);
                }
            }
        }
        
        // Подсчитываем коржи в стопке
        int stackCount = CountKorzhsInStack();
        
        Debug.Log($"Проверка заказа: коржей в стопке = {stackCount}");
        
        // Проверяем заказ
        if (OrderSystem.Instance != null)
        {
            bool isCorrect = OrderSystem.Instance.CheckOrder(stackCount);
            
            if (isCorrect)
            {
                // ПРАВИЛЬНО
                Debug.Log($"✅ ЗАКАЗ ПРАВИЛЬНЫЙ! {stackCount} коржей");
                RemoveAllKorzhs(); // Удаляем торт
                OrderSystem.Instance.CompleteOrderCorrect(); // +1 очко
            }
            else
            {
                // НЕПРАВИЛЬНО
                Debug.Log($"❌ ЗАКАЗ НЕПРАВИЛЬНЫЙ! Нужно: {OrderSystem.Instance.GetCurrentOrderKorzhCount()}, Дано: {stackCount}");
                RemoveAllKorzhs(); // Все равно удаляем торт
                OrderSystem.Instance.CompleteOrderIncorrect(stackCount); // Без очков
            }
        }
    }
    
    /// <summary>
    /// Подсчитать коржи в стопке (только те что лежат друг на друге)
    /// </summary>
    int CountKorzhsInStack()
    {
        if (korzhsOnPlate.Count == 0) return 0;
        
        // Сортируем коржи по высоте (Y координата)
        korzhsOnPlate.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
        
        // Начинаем с самого нижнего коржа
        List<GameObject> stack = new List<GameObject>();
        stack.Add(korzhsOnPlate[0]);
        
        // Проверяем каждый следующий корж
        for (int i = 1; i < korzhsOnPlate.Count; i++)
        {
            GameObject currentKorzh = korzhsOnPlate[i];
            GameObject previousKorzh = stack[stack.Count - 1];
            
            // Вычисляем расстояние между коржами
            float distance = currentKorzh.transform.position.y - previousKorzh.transform.position.y;
            
            // Если коржи близко друг к другу - они в стопке
            if (distance <= stackTolerance)
            {
                stack.Add(currentKorzh);
            }
            else
            {
                // Если расстояние большое - стопка прервана
                // Начинаем новую стопку с текущего коржа
                stack.Clear();
                stack.Add(currentKorzh);
            }
        }
        
        return stack.Count;
    }
    
    /// <summary>
    /// Удалить все коржи с тарелки
    /// </summary>
    void RemoveAllKorzhs()
    {
        // Удаляем все коржи на тарелке
        foreach (GameObject korzh in korzhsOnPlate)
        {
            if (korzh != null)
            {
                Destroy(korzh);
            }
        }
        
        korzhsOnPlate.Clear();
        Debug.Log("🗑️ Все коржи удалены с тарелки");
    }
    
    /// <summary>
    /// Визуализация радиуса проверки в редакторе
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
