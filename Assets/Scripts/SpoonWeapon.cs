using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ложка-оружие для отпугивания тараканов
/// </summary>
public class SpoonWeapon : MonoBehaviour
{
    [Header("Настройки отпугивания")]
    [Tooltip("Радиус отпугивания вокруг ложки")]
    public float scareRadius = 3f;
    
    [Tooltip("Сила отталкивания тараканов")]
    public float scareForce = 5f;
    
    [Tooltip("Длительность эффекта испуга (секунды)")]
    public float scareDuration = 2f;
    
    [Header("Визуализация")]
    [Tooltip("Показывать радиус отпугивания в редакторе")]
    public bool showScareRadius = true;
    
    [Tooltip("Цвет радиуса отпугивания")]
    public Color scareRadiusColor = Color.red;
    
    private bool isHeldByPlayer = false;
    
    void Update()
    {
        // Проверяем, держит ли игрок ложку
        CheckIfHeldByPlayer();
        
        // Если ложка в руках - отпугиваем тараканов
        if (isHeldByPlayer)
        {
            ScareNearbyEnemies();
        }
    }
    
    /// <summary>
    /// Проверяет, держит ли игрок ложку
    /// </summary>
    void CheckIfHeldByPlayer()
    {
        PickupObject pickup = GetComponent<PickupObject>();
        if (pickup != null)
        {
            isHeldByPlayer = pickup.isPickedUp;
        }
    }
    
    /// <summary>
    /// Отпугивает тараканов в радиусе
    /// </summary>
    void ScareNearbyEnemies()
    {
        // Находим всех тараканов в радиусе
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, scareRadius);
        
        foreach (Collider2D col in colliders)
        {
            // Проверяем, что это таракан
            EnemyMove enemy = col.GetComponent<EnemyMove>();
            if (enemy != null)
            {
                // Отпугиваем таракана
                ScareEnemy(enemy);
            }
        }
    }
    
    /// <summary>
    /// Отпугивает конкретного таракана
    /// </summary>
    void ScareEnemy(EnemyMove enemy)
    {
        // Вычисляем направление от ложки к таракану
        Vector2 scareDirection = (enemy.transform.position - transform.position).normalized;
        
        // Применяем силу отталкивания
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            // Отталкиваем таракана
            enemyRb.AddForce(scareDirection * scareForce, ForceMode2D.Impulse);
        }
        
        // Уведомляем таракана, что он испуган
        enemy.GetScared(scareDuration);
    }
    
    /// <summary>
    /// Визуализация радиуса отпугивания в редакторе
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (showScareRadius)
        {
            Gizmos.color = scareRadiusColor;
            Gizmos.DrawWireSphere(transform.position, scareRadius);
        }
    }
}
