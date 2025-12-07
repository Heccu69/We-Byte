using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [Tooltip("Префаб таракана")]
    public GameObject enemyPrefab;
    
    [Tooltip("Минимальное время до следующего спавна (секунды)")]
    public float minSpawnInterval = 5f;
    
    [Tooltip("Максимальное время до следующего спавна (секунды)")]
    public float maxSpawnInterval = 15f;
    
    [Header("Область спавна")]
    [Tooltip("Минимальная позиция X для спавна")]
    public float spawnMinX = -8f;
    
    [Tooltip("Максимальная позиция X для спавна")]
    public float spawnMaxX = 8f;
    
    [Tooltip("Минимальная позиция Y для спавна")]
    public float spawnMinY = -4f;
    
    [Tooltip("Максимальная позиция Y для спавна")]
    public float spawnMaxY = 4f;
    
    [Header("Ограничения")]
    [Tooltip("Максимальное количество тараканов одновременно")]
    public int maxEnemies = 5;
    
    [Tooltip("Автоматически начать спавн при старте")]
    public bool autoStart = true;
    
    private int currentEnemyCount = 0;
    private bool isSpawning = false;
    
    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab не назначен в EnemySpawner!");
            return;
        }
        
        if (autoStart)
        {
            StartSpawning();
        }
    }
    
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemiesRoutine());
            Debug.Log("Спавн тараканов запущен");
        }
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
        Debug.Log("Спавн тараканов остановлен");
    }
    
    IEnumerator SpawnEnemiesRoutine()
    {
        while (isSpawning)
        {
            // Случайная задержка до следующего спавна
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
            
            // Проверяем лимит тараканов
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
            else
            {
                Debug.Log($"Достигнут лимит тараканов: {maxEnemies}");
            }
        }
    }
    
    void SpawnEnemy()
    {
        // Случайная позиция в заданной области
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnMinX, spawnMaxX),
            Random.Range(spawnMinY, spawnMaxY),
            0f
        );
        
        // Создаем таракана
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.name = $"Enemy_{currentEnemyCount + 1}";
        
        // Настраиваем физику таракана
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 3f; // Увеличенная гравитация для быстрого падения
        }
        
        // Подписываемся на уничтожение таракана
        EnemyDeathNotifier notifier = enemy.GetComponent<EnemyDeathNotifier>();
        if (notifier == null)
        {
            notifier = enemy.AddComponent<EnemyDeathNotifier>();
        }
        notifier.OnEnemyDestroyed += OnEnemyDestroyed;
        
        currentEnemyCount++;
        Debug.Log($"Таракан заспавнен на позиции {spawnPosition}. Всего тараканов: {currentEnemyCount}");
    }
    
    void OnEnemyDestroyed()
    {
        currentEnemyCount--;
        if (currentEnemyCount < 0) currentEnemyCount = 0;
        Debug.Log($"Таракан уничтожен. Осталось тараканов: {currentEnemyCount}");
    }
    
    // Визуализация области спавна в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        
        Vector3 center = new Vector3(
            (spawnMinX + spawnMaxX) / 2f,
            (spawnMinY + spawnMaxY) / 2f,
            0f
        );
        
        Vector3 size = new Vector3(
            spawnMaxX - spawnMinX,
            spawnMaxY - spawnMinY,
            0f
        );
        
        Gizmos.DrawWireCube(center, size);
        
        // Рисуем углы
        Gizmos.color = Color.red;
        float cornerSize = 0.3f;
        
        // Левый нижний
        Gizmos.DrawSphere(new Vector3(spawnMinX, spawnMinY, 0), cornerSize);
        // Правый нижний
        Gizmos.DrawSphere(new Vector3(spawnMaxX, spawnMinY, 0), cornerSize);
        // Левый верхний
        Gizmos.DrawSphere(new Vector3(spawnMinX, spawnMaxY, 0), cornerSize);
        // Правый верхний
        Gizmos.DrawSphere(new Vector3(spawnMaxX, spawnMaxY, 0), cornerSize);
    }
}

// Компонент для уведомления о смерти таракана
public class EnemyDeathNotifier : MonoBehaviour
{
    public System.Action OnEnemyDestroyed;
    
    void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke();
    }
}
