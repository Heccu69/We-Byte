using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Тарелка для сборки торта - подсчитывает коржи в стопке
/// </summary>
public class UnderCakePlate : MonoBehaviour
{
    [Header("Настройки")]
    public Vector2 checkBoxSize = new Vector2(1.5f, 3f); // Размер зоны проверки (ширина, высота)
    public Vector2 checkBoxOffset = new Vector2(0f, 1.5f); // Смещение зоны вверх от тарелки
    public float stackTolerance = 1.0f; // Допуск для определения стопки (расстояние между коржами)
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
        
        // Находим все коржи в прямоугольной зоне над тарелкой
        Vector2 boxCenter = (Vector2)transform.position + checkBoxOffset;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, checkBoxSize, 0f);
        
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
        
        Debug.Log($"🔍 Найдено коржей в зоне: {korzhsOnPlate.Count}");
        
        // Подсчитываем коржи в стопке
        int stackCount = CountKorzhsInStack();
        
        Debug.Log($"🎂 Проверка заказа: коржей в стопке = {stackCount}");
        
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
        
        Debug.Log("📊 Анализ стопки:");
        
        // Начинаем с самого нижнего коржа
        List<GameObject> stack = new List<GameObject>();
        stack.Add(korzhsOnPlate[0]);
        Debug.Log($"  ✅ Корж 1: {korzhsOnPlate[0].name} Y={korzhsOnPlate[0].transform.position.y:F2}");
        
        // Проверяем каждый следующий корж
        for (int i = 1; i < korzhsOnPlate.Count; i++)
        {
            GameObject currentKorzh = korzhsOnPlate[i];
            GameObject previousKorzh = stack[stack.Count - 1];
            
            // Вычисляем расстояние между коржами
            float distance = currentKorzh.transform.position.y - previousKorzh.transform.position.y;
            
            Debug.Log($"  📏 Корж {i+1}: {currentKorzh.name} Y={currentKorzh.transform.position.y:F2}, расстояние={distance:F2}");
            
            // Если коржи близко друг к другу - они в стопке
            if (distance <= stackTolerance)
            {
                stack.Add(currentKorzh);
                Debug.Log($"    ✅ Добавлен в стопку (расстояние {distance:F2} <= {stackTolerance})");
            }
            else
            {
                // Если расстояние большое - стопка прервана
                Debug.Log($"    ❌ Стопка прервана! (расстояние {distance:F2} > {stackTolerance}). Сброс счетчика.");
                // Начинаем новую стопку с текущего коржа
                stack.Clear();
                stack.Add(currentKorzh);
            }
        }
        
        Debug.Log($"✅ Итого в стопке: {stack.Count} коржей");
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
    /// Визуализация зоны проверки в редакторе
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 boxCenter = transform.position + (Vector3)checkBoxOffset;
        Gizmos.DrawWireCube(boxCenter, checkBoxSize);
    }
}
