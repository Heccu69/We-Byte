using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    public GameObject korzhPrefab; // Префаб коржа (если есть)
    public float spawnInterval = 2f; // Интервал между спавнами (в секундах)
    public float spawnOffsetY = 0f; // Смещение по Y относительно спавнера
    
    [Header("Спрайты коржей")]
    public Sprite[] korzhSprites; // Массив спрайтов коржей для случайного выбора
    
    [Header("Платформа конвейера")]
    public GameObject platformPrefab; // Префаб платформы
    public Sprite platformSprite; // Спрайт платформы
    public Vector3 platformOffset = new Vector3(0, -0.15f, 0); // Смещение платформы относительно коржа
    
    [Header("Ограничения")]
    public int maxActivePairs = 5; // Максимальное количество активных пар
    public int maxPlatformsInScene = 5; // Максимум платформ в иерархии
    public int maxKorzhsInScene = 25; // Максимум коржей в иерархии
    
    private float nextSpawnTime = 0f;
    
    // Список активных пар (платформа, корж)
    private List<ConveyorPairData> activePairs = new List<ConveyorPairData>();
    
    // Списки для отслеживания порядка создания
    private List<GameObject> allPlatforms = new List<GameObject>(); // Все платформы в порядке создания
    private List<GameObject> allKorzhs = new List<GameObject>(); // Все коржи в порядке создания
    
    [System.Serializable]
    public class ConveyorPairData
    {
        public GameObject platform;
        public GameObject korzh;
        
        public ConveyorPairData(GameObject platform, GameObject korzh)
        {
            this.platform = platform;
            this.korzh = korzh;
        }
        
        public bool IsValid()
        {
            return platform != null && korzh != null;
        }
    }
    
    void Update()
    {
        // Проверяем, пора ли спавнить новый объект
        if (Time.time >= nextSpawnTime)
        {
            SpawnKorzh();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
    
    void SpawnKorzh()
    {
        // Очищаем список от удаленных объектов
        CleanupInvalidPairs();
        
        // Проверяем ЖЕСТКИЕ лимиты на количество объектов в сцене
        EnforceSceneLimits();
        
        // Проверяем лимит пар - если превышен, удаляем самую старую пару
        if (activePairs.Count >= maxActivePairs)
        {
            RemoveOldestPair();
        }
        
        Vector3 platformSpawnPos = transform.position + new Vector3(0, spawnOffsetY, 0);
        
        if (korzhPrefab == null)
        {
            Debug.LogWarning("Префаб коржа не задан! Назначьте Korzh Prefab в инспекторе.");
            return;
        }
        
        // Сначала создаем платформу
        GameObject platform = CreatePlatform(platformSpawnPos);
        if (platform == null)
        {
            Debug.LogWarning("Не удалось создать платформу!");
            return;
        }
        
        // Теперь создаем корж ВЫШЕ платформы (чтобы не застрял внутри)
        float korzhHeight = 0.6f; // Увеличенная высота
        Vector3 korzhSpawnPos = platformSpawnPos + new Vector3(0, korzhHeight, 0);
        
        GameObject korzh = Instantiate(korzhPrefab, korzhSpawnPos, Quaternion.identity);
        
        // Настраиваем Rigidbody2D для складывания и толкания
        Rigidbody2D rb = korzh.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Dynamic для физики
            rb.gravityScale = 1f;
            rb.mass = 1.5f; // Меньше массы игрока (3) - можно толкнуть
            rb.drag = 0.5f; // Сопротивление - корж быстро останавливается
            rb.angularDrag = 0.5f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Не вращаться
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        
        // Настраиваем коллайдер для складывания
        BoxCollider2D col = korzh.GetComponent<BoxCollider2D>();
        if (col != null)
        {
            col.isTrigger = false; // НЕ триггер - физические столкновения
        }
        
        // Добавляем тег для конвейера
        korzh.tag = "ConveyorObject";
        
        // Добавляем компонент для связи с платформой
        ConveyorPairLink korzhLink = korzh.AddComponent<ConveyorPairLink>();
        korzhLink.pairedObject = platform;
        
        ConveyorPairLink platformLink = platform.AddComponent<ConveyorPairLink>();
        platformLink.pairedObject = korzh;
        
        // Регистрируем пару
        ConveyorPairData pair = new ConveyorPairData(platform, korzh);
        activePairs.Add(pair);
        
        // Регистрируем объекты в списках (в порядке создания)
        allPlatforms.Add(platform);
        allKorzhs.Add(korzh);
        
        // Устанавливаем случайный спрайт
        if (korzhSprites != null && korzhSprites.Length > 0)
        {
            SpriteRenderer sr = korzh.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = korzhSprites[Random.Range(0, korzhSprites.Length)];
            }
        }
        
    }
    
    GameObject CreatePlatform(Vector3 position)
    {
        GameObject platform;
        
        if (platformPrefab != null)
        {
            // Используем префаб платформы
            platform = Instantiate(platformPrefab, position, Quaternion.identity);
        }
        else if (platformSprite != null)
        {
            // Создаем платформу со спрайтом
            platform = new GameObject("ConveyorPlatform");
            platform.transform.position = position;
            
            SpriteRenderer sr = platform.AddComponent<SpriteRenderer>();
            sr.sprite = platformSprite;
            sr.sortingOrder = -1; // Платформа позади коржа
        }
        else
        {
            return null;
        }
        
        // Добавляем компонент ConveyorPlatform (он настроит физику)
        platform.AddComponent<ConveyorPlatform>();
        
        // Добавляем тег для конвейера
        platform.tag = "ConveyorObject";
        
        return platform;
    }
    
    // Очистка списка от удаленных объектов
    void CleanupInvalidPairs()
    {
        activePairs.RemoveAll(pair => !pair.IsValid());
    }
    
    // Подсчет всех платформ в сцене
    int CountPlatformsInScene()
    {
        // Очищаем список от null
        allPlatforms.RemoveAll(p => p == null);
        return allPlatforms.Count;
    }
    
    // Подсчет всех коржей в сцене
    int CountKorzhsInScene()
    {
        // Очищаем список от null
        allKorzhs.RemoveAll(k => k == null);
        return allKorzhs.Count;
    }
    
    // Принудительное соблюдение лимитов в сцене
    void EnforceSceneLimits()
    {
        // Проверяем платформы
        int platformCount = CountPlatformsInScene();
        if (platformCount >= maxPlatformsInScene)
        {
            RemoveOldestPlatforms(platformCount - maxPlatformsInScene + 1);
        }
        
        // Проверяем коржи
        int korzhCount = CountKorzhsInScene();
        if (korzhCount >= maxKorzhsInScene)
        {
            RemoveOldestKorzhs(korzhCount - maxKorzhsInScene + 1);
        }
    }
    
    // Удаление старых платформ (самые первые в списке)
    void RemoveOldestPlatforms(int count)
    {
        // Очищаем список от null
        allPlatforms.RemoveAll(p => p == null);
        
        // Удаляем первые N платформ (самые старые)
        int toRemove = Mathf.Min(count, allPlatforms.Count);
        for (int i = 0; i < toRemove; i++)
        {
            if (allPlatforms[0] != null)
            {
                Destroy(allPlatforms[0]);
            }
            allPlatforms.RemoveAt(0); // Удаляем из списка
        }
    }
    
    // Удаление старых коржей (кроме подобранных)
    void RemoveOldestKorzhs(int count)
    {
        // Очищаем список от null
        allKorzhs.RemoveAll(k => k == null);
        
        int removed = 0;
        int index = 0;
        
        // Удаляем старые коржи (кроме подобранных)
        while (removed < count && index < allKorzhs.Count)
        {
            GameObject korzhObj = allKorzhs[index];
            
            if (korzhObj != null)
            {
                PickupObject pickupObj = korzhObj.GetComponent<PickupObject>();
                
                // Удаляем только НЕ подобранные
                if (pickupObj == null || !pickupObj.isPickedUp)
                {
                    Destroy(korzhObj);
                    allKorzhs.RemoveAt(index);
                    removed++;
                }
                else
                {
                    index++; // Пропускаем подобранный
                }
            }
            else
            {
                allKorzhs.RemoveAt(index); // Удаляем null
            }
        }
    }
    
    // Удаление самой старой пары (первой в списке)
    void RemoveOldestPair()
    {
        if (activePairs.Count == 0) return;
        
        // Самая старая пара - первая в списке
        ConveyorPairData oldestPair = activePairs[0];
        
        // Удаляем оба объекта
        if (oldestPair.platform != null)
        {
            Destroy(oldestPair.platform);
        }
        
        if (oldestPair.korzh != null)
        {
            // Проверяем, не подобран ли корж
            PickupObject pickupObj = oldestPair.korzh.GetComponent<PickupObject>();
            if (pickupObj == null || !pickupObj.isPickedUp)
            {
                Destroy(oldestPair.korzh);
            }
            else
            {
                // Корж подобран - не удаляем
            }
        }
        
        // Удаляем из списка
        activePairs.RemoveAt(0);
    }
    
    // Публичный метод для удаления пары
    public void RemovePair(GameObject obj)
    {
        activePairs.RemoveAll(pair => pair.platform == obj || pair.korzh == obj);
    }
    
    // Визуализация точки спавна в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 spawnPos = transform.position + new Vector3(0, spawnOffsetY, 0);
        Gizmos.DrawWireCube(spawnPos, new Vector3(1f, 0.3f, 0.1f));
        
        // Рисуем крест в точке спавна
        Gizmos.DrawLine(spawnPos + Vector3.left * 0.3f, spawnPos + Vector3.right * 0.3f);
        Gizmos.DrawLine(spawnPos + Vector3.down * 0.3f, spawnPos + Vector3.up * 0.3f);
        
        // Показываем позицию платформы
        Gizmos.color = Color.blue;
        Vector3 platformPos = spawnPos + platformOffset;
        Gizmos.DrawWireCube(platformPos, new Vector3(1f, 0.2f, 0.1f));
    }
}
