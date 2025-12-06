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
    
    private float nextSpawnTime = 0f;
    
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
        
        // Настраиваем Rigidbody2D для полной физики
        Rigidbody2D rb = korzh.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f; // Корж упадет на платформу
            rb.mass = 1f;
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
        }
        
        // Добавляем тег для конвейера
        korzh.tag = "ConveyorObject";
        
        // Устанавливаем случайный спрайт
        if (korzhSprites != null && korzhSprites.Length > 0)
        {
            SpriteRenderer sr = korzh.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = korzhSprites[Random.Range(0, korzhSprites.Length)];
            }
        }
        
        Debug.Log($"Заспавнены: платформа на {platformSpawnPos}, корж на {korzhSpawnPos}");
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
