using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет тортом и взаимодействием с тараканами
/// </summary>
public class CakeManager : MonoBehaviour
{
    [Header("Настройки торта")]
    [Tooltip("Тарелка (UnderCake) - точка, куда идут тараканы")]
    public Transform underCakePlate;
    
    [Tooltip("Список коржей на торте (снизу вверх)")]
    public List<GameObject> cakeLayers = new List<GameObject>();
    
    [Header("Настройки утаскивания")]
    [Tooltip("Базовая скорость утаскивания коржа (единиц в секунду)")]
    public float baseStealSpeed = 0.5f;
    
    [Tooltip("Дополнительная скорость за каждого таракана")]
    public float speedPerEnemy = 0.3f;
    
    [Tooltip("Расстояние между тараканами в очереди")]
    public float queueSpacing = 0.5f;
    
    [Header("Позиции очереди")]
    [Tooltip("Смещение первого таракана от тарелки")]
    public Vector3 firstEnemyOffset = new Vector3(0.5f, 0f, 0f);
    
    // ОТКЛЮЧЕНО: Система кражи коржей деактивирована
    // Это поле больше не используется
    private List<EnemyMove> enemiesAtCake = new List<EnemyMove>();

    void Start()
    {
        // ОТКЛЮЧЕНО: Система кражи коржей деактивирована
        // Тараканы просто бегают по столу
        Debug.Log("CakeManager: Система кражи коржей отключена. Тараканы просто бегают по столу.");
        
        /*
        // Находим тарелку, если не назначена
        if (underCakePlate == null)
        {
            GameObject underCake = GameObject.Find("UnderCake");
            if (underCake != null)
            {
                underCakePlate = underCake.transform;
                Debug.Log($"CakeManager нашел тарелку: {underCake.name}");
            }
            else
            {
                Debug.LogWarning("Тарелка UnderCake не найдена!");
            }
        }
        
        // Автоматически собираем коржи, если список пуст
        if (cakeLayers.Count == 0)
        {
            AutoCollectCakeLayers();
        }
        */
    }
    
    void AutoCollectCakeLayers()
    {
        Debug.Log("=== Начинаем автоматический сбор коржей ===");
        
        // Ищем все объекты с компонентом PickupObject
        PickupObject[] allPickups = FindObjectsOfType<PickupObject>();
        Debug.Log($"Найдено объектов PickupObject: {allPickups.Length}");
        
        foreach (PickupObject pickup in allPickups)
        {
            Debug.Log($"Проверяем объект: {pickup.gameObject.name}, тип: {pickup.objectType}");
            
            if (pickup.objectType == ObjectType.Korzh)
            {
                // Проверяем, что корж НЕ подобран игроком
                if (pickup.isPickedUp)
                {
                    Debug.Log($"  ✗ Корж {pickup.gameObject.name} в руках игрока - пропускаем");
                    continue;
                }
                
                // Проверяем, что корж НЕ на конвейере (не связан с платформой)
                ConveyorPairLink pairLink = pickup.GetComponent<ConveyorPairLink>();
                if (pairLink != null && pairLink.pairedObject != null)
                {
                    Debug.Log($"  ✗ Корж {pickup.gameObject.name} на конвейере (связан с платформой) - пропускаем");
                    continue;
                }
                
                // Проверяем, что корж не движется (остановился)
                Rigidbody2D rb = pickup.GetComponent<Rigidbody2D>();
                if (rb != null && rb.velocity.magnitude > 0.1f)
                {
                    Debug.Log($"  ✗ Корж {pickup.gameObject.name} движется (скорость {rb.velocity.magnitude:F2}) - пропускаем");
                    continue;
                }
                
                // Проверяем, что корж находится на торте (рядом с тарелкой)
                if (underCakePlate != null)
                {
                    float distance = Vector3.Distance(pickup.transform.position, underCakePlate.position);
                    Debug.Log($"  - Корж {pickup.gameObject.name} на расстоянии {distance:F2} от тарелки");
                    
                    if (distance < 9f) // Увеличен радиус до 9 единиц
                    {
                        cakeLayers.Add(pickup.gameObject);
                        Debug.Log($"  ✓ Корж {pickup.gameObject.name} добавлен в список!");
                    }
                    else
                    {
                        Debug.Log($"  ✗ Корж {pickup.gameObject.name} слишком далеко от тарелки");
                    }
                }
                else
                {
                    Debug.LogWarning("  ✗ Тарелка не назначена!");
                }
            }
        }
        
        // Сортируем коржи по высоте (снизу вверх)
        cakeLayers.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
        
        Debug.Log($"=== CakeManager собрал {cakeLayers.Count} коржей ===");
        
        if (cakeLayers.Count == 0)
        {
            Debug.LogWarning("⚠️ Коржи не найдены! Проверьте:");
            Debug.LogWarning("  1. Есть ли объекты с компонентом PickupObject и типом Korzh");
            Debug.LogWarning("  2. Находятся ли они рядом с тарелкой (в радиусе 9 единиц)");
            Debug.LogWarning("  3. Не движутся ли коржи и не связаны ли с платформой");
            Debug.LogWarning("  4. Назначена ли тарелка в CakeManager");
        }
    }

    /// <summary>
    /// Регистрирует таракана, достигшего торта
    /// ОТКЛЮЧЕНО - тараканы просто бегают
    /// </summary>
    public void RegisterEnemyAtCake(EnemyMove enemy)
    {
        // ОТКЛЮЧЕНО: Тараканы не взаимодействуют с тортом
        // Просто бегают по столу
    }
    
    /// <summary>
    /// Удаляет таракана из очереди
    /// ОТКЛЮЧЕНО - тараканы просто бегают
    /// </summary>
    public void UnregisterEnemy(EnemyMove enemy)
    {
        // ОТКЛЮЧЕНО: Тараканы не взаимодействуют с тортом
    }
    
    /// <summary>
    /// Обновляет позиции тараканов в очереди
    /// ОТКЛЮЧЕНО - тараканы просто бегают
    /// </summary>
    void UpdateEnemyQueuePositions()
    {
        // ОТКЛЮЧЕНО: Тараканы не выстраиваются в очередь
    }
    
    /// <summary>
    /// Процесс утаскивания коржа
    /// ОТКЛЮЧЕНО - тараканы не крадут коржи
    /// </summary>
    IEnumerator StealCakeLayer()
    {
        // ОТКЛЮЧЕНО: Утаскивание коржей деактивировано
        yield break;
    }
    
    /// <summary>
    /// Добавляет новый корж на торт
    /// </summary>
    public void AddCakeLayer(GameObject layer)
    {
        if (!cakeLayers.Contains(layer))
        {
            cakeLayers.Add(layer);
            // Пересортировываем по высоте
            cakeLayers.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
            Debug.Log($"Добавлен новый корж. Всего коржей: {cakeLayers.Count}");
        }
    }
    
    /// <summary>
    /// Обновляет список коржей (пересканирует область вокруг тарелки)
    /// </summary>
    public void UpdateCakeLayers()
    {
        AutoCollectCakeLayers();
    }
    
    /// <summary>
    /// Вызывается каждые несколько секунд для обновления списка коржей
    /// ОТКЛЮЧЕНО - тараканы не крадут коржи
    /// </summary>
    void Update()
    {
        // ОТКЛЮЧЕНО: Автоматический сбор коржей не нужен
    }
    
    /// <summary>
    /// Возвращает позицию тарелки
    /// </summary>
    public Vector3 GetPlatePosition()
    {
        return underCakePlate != null ? underCakePlate.position : transform.position;
    }
    
    /// <summary>
    /// Проверяет, есть ли еще коржи
    /// </summary>
    public bool HasCakeLayers()
    {
        return cakeLayers.Count > 0;
    }
}
