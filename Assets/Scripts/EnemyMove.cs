using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Rigidbody2D rgb;
    public Transform cake;
    public SpriteRenderer spriteRenderer;
    public float _speed = 0.5f;
    
    // Расстояние, на котором враг останавливается (подберите значение под ваш масштаб)
    public float stopDistance = 0.2f;
    
    // Менеджер торта
    private CakeManager cakeManager;
    
    // Флаг достижения торта
    private bool hasReachedCake = false;
    
    // Позиция в очереди
    private Vector3? queuePosition = null;

    private void Start()
    {
        // Автоматически находим тарелку (UnderCake), если не назначен
        if (cake == null)
        {
            // Сначала пробуем найти по тегу
            GameObject cakeObject = GameObject.FindGameObjectWithTag("Cake");
            if (cakeObject != null)
            {
                cake = cakeObject.transform;
                Debug.Log($"Таракан нашел тарелку: {cakeObject.name}");
            }
            else
            {
                // Если не нашли по тегу, ищем по имени
                GameObject underCake = GameObject.Find("UnderCake");
                if (underCake != null)
                {
                    cake = underCake.transform;
                    Debug.Log($"Таракан нашел тарелку по имени: {underCake.name}");
                }
                else
                {
                    Debug.LogWarning("Тарелка не найдена! Убедитесь, что у объекта UnderCake есть тег 'Cake' или имя 'UnderCake'");
                }
            }
        }
        
        // Настраиваем гравитацию для быстрого падения
        if (rgb != null)
        {
            rgb.gravityScale = 3f; // Увеличенная гравитация
        }
        
        // Сразу поворачиваемся к торту при спавне
        if (cake != null && spriteRenderer != null)
        {
            spriteRenderer.flipX = (cake.position.x > transform.position.x);
        }
        
        // Находим менеджер торта
        cakeManager = FindObjectOfType<CakeManager>();
        if (cakeManager == null)
        {
            Debug.LogWarning("CakeManager не найден! Создайте объект с компонентом CakeManager");
        }
        
        // Игнорируем коллизии с коржами, чтобы тараканы могли подойти к тарелке
        IgnoreKorzhCollisions();
    }
    
    void IgnoreKorzhCollisions()
    {
        // Находим все коржи
        GameObject[] korzhs = GameObject.FindGameObjectsWithTag("ConveyorObject");
        Collider2D enemyCollider = GetComponent<Collider2D>();
        
        if (enemyCollider == null) return;
        
        foreach (GameObject korzh in korzhs)
        {
            Collider2D korzhCollider = korzh.GetComponent<Collider2D>();
            if (korzhCollider != null)
            {
                Physics2D.IgnoreCollision(enemyCollider, korzhCollider);
            }
        }
        
        // Также ищем по компоненту PickupObject
        PickupObject[] allPickups = FindObjectsOfType<PickupObject>();
        foreach (PickupObject pickup in allPickups)
        {
            if (pickup.objectType == ObjectType.Korzh)
            {
                Collider2D korzhCollider = pickup.GetComponent<Collider2D>();
                if (korzhCollider != null)
                {
                    Physics2D.IgnoreCollision(enemyCollider, korzhCollider);
                }
            }
        }
    }

    private void Update()
    {
        // Проверяем, что торт назначен
        if (cake == null) return;
        
        // Определяем целевую позицию
        Vector3 targetPosition;
        
        if (queuePosition.HasValue)
        {
            // Если есть позиция в очереди, идем к ней
            targetPosition = queuePosition.Value;
        }
        else
        {
            // Иначе идем к торту
            targetPosition = cake.position;
        }
        
        // Вычисляем вектор направления
        Vector2 direction = (targetPosition - transform.position).normalized;
        
        // Вычисляем расстояние
        float distance = Vector2.Distance(transform.position, targetPosition);
        
        if (distance > stopDistance)
        {
            // Если дальше порога — двигаемся
            rgb.velocity = direction * _speed;
        }
        else
        {
            // Если близко — останавливаемся
            rgb.velocity = Vector2.zero;
            
            // Если достигли торта и еще не зарегистрировались
            if (!hasReachedCake && !queuePosition.HasValue && cakeManager != null)
            {
                hasReachedCake = true;
                cakeManager.RegisterEnemyAtCake(this);
            }
        }

        // Поворот спрайта в сторону цели (инвертировано)
        if (targetPosition.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    
    /// <summary>
    /// Устанавливает позицию в очереди (вызывается CakeManager)
    /// </summary>
    public void SetQueuePosition(Vector3 position)
    {
        queuePosition = position;
    }
    
    private void OnDestroy()
    {
        // Отписываемся от менеджера при уничтожении
        if (cakeManager != null && hasReachedCake)
        {
            cakeManager.UnregisterEnemy(this);
        }
    }
}
