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
    
    // Очередь тараканов у торта
    private List<EnemyMove> enemiesAtCake = new List<EnemyMove>();
    
    // Флаг процесса утаскивания
    private bool isStealingInProgress = false;
    
    // Текущий утаскиваемый корж
    private GameObject currentStolenLayer = null;

    void Start()
    {
        // Автоматически находим тарелку, если не назначена
        if (underCakePlate == null)
        {
            GameObject plate = GameObject.Find("UnderCake");
            if (plate != null)
            {
                underCakePlate = plate.transform;
                Debug.Log($"CakeManager нашел тарелку: {plate.name}");
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
    /// </summary>
    public void RegisterEnemyAtCake(EnemyMove enemy)
    {
        if (!enemiesAtCake.Contains(enemy))
        {
            enemiesAtCake.Add(enemy);
            Debug.Log($"Таракан добавлен в очередь. Всего тараканов: {enemiesAtCake.Count}");
            
            // Назначаем позицию в очереди
            UpdateEnemyQueuePositions();
            
            // Если это первый таракан, начинаем утаскивание
            if (enemiesAtCake.Count == 1 && !isStealingInProgress)
            {
                StartCoroutine(StealCakeLayer());
            }
        }
    }
    
    /// <summary>
    /// Удаляет таракана из очереди
    /// </summary>
    public void UnregisterEnemy(EnemyMove enemy)
    {
        if (enemiesAtCake.Contains(enemy))
        {
            enemiesAtCake.Remove(enemy);
            Debug.Log($"Таракан удален из очереди. Осталось тараканов: {enemiesAtCake.Count}");
            
            UpdateEnemyQueuePositions();
        }
    }
    
    /// <summary>
    /// Обновляет позиции тараканов в очереди
    /// </summary>
    void UpdateEnemyQueuePositions()
    {
        if (underCakePlate == null) return;
        
        for (int i = 0; i < enemiesAtCake.Count; i++)
        {
            if (enemiesAtCake[i] != null)
            {
                // Позиция в очереди: первый у тарелки, остальные за ним
                Vector3 queuePosition = underCakePlate.position + firstEnemyOffset + Vector3.right * (i * queueSpacing);
                enemiesAtCake[i].SetQueuePosition(queuePosition);
            }
        }
    }
    
    /// <summary>
    /// Процесс утаскивания коржа
    /// </summary>
    IEnumerator StealCakeLayer()
    {
        while (enemiesAtCake.Count > 0 && cakeLayers.Count > 0)
        {
            isStealingInProgress = true;
            
            // Берем нижний корж
            GameObject bottomLayer = cakeLayers[0];
            
            if (bottomLayer == null)
            {
                // Корж был уничтожен, удаляем из списка
                cakeLayers.RemoveAt(0);
                continue;
            }
            
            currentStolenLayer = bottomLayer;
            
            // Вычисляем скорость утаскивания (зависит от количества тараканов)
            float stealSpeed = baseStealSpeed + (enemiesAtCake.Count - 1) * speedPerEnemy;
            
            Debug.Log($"Тараканы утаскивают корж! Скорость: {stealSpeed} (тараканов: {enemiesAtCake.Count})");
            
            // Отключаем физику коржа
            Rigidbody2D rb = bottomLayer.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = Vector2.zero;
            }
            
            // Отключаем коллайдер
            Collider2D col = bottomLayer.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
            
            // Утаскиваем корж вправо
            Vector3 startPos = bottomLayer.transform.position;
            float stealDistance = 5f; // Расстояние утаскивания
            Vector3 targetPos = startPos + Vector3.right * stealDistance;
            
            float elapsedTime = 0f;
            float duration = stealDistance / stealSpeed;
            
            while (elapsedTime < duration)
            {
                // Обновляем скорость, если количество тараканов изменилось
                stealSpeed = baseStealSpeed + (enemiesAtCake.Count - 1) * speedPerEnemy;
                duration = stealDistance / stealSpeed;
                
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                
                if (bottomLayer != null)
                {
                    bottomLayer.transform.position = Vector3.Lerp(startPos, targetPos, t);
                }
                else
                {
                    break;
                }
                
                yield return null;
            }
            
            // Уничтожаем корж
            if (bottomLayer != null)
            {
                Destroy(bottomLayer);
            }
            
            // Удаляем из списка
            cakeLayers.RemoveAt(0);
            currentStolenLayer = null;
            
            Debug.Log($"Корж утащен! Осталось коржей: {cakeLayers.Count}");
            
            // Небольшая пауза перед следующим коржом
            yield return new WaitForSeconds(0.5f);
        }
        
        isStealingInProgress = false;
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
    /// </summary>
    void Update()
    {
        // Обновляем список коржей каждые 2 секунды
        if (Time.frameCount % 120 == 0) // Примерно каждые 2 секунды при 60 FPS
        {
            // Только если список пуст или тараканы уже у торта
            if (cakeLayers.Count == 0 || enemiesAtCake.Count > 0)
            {
                AutoCollectCakeLayers();
            }
        }
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
