using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Настройки следования")]
    [Tooltip("Цель, за которой следует камера (игрок)")]
    public Transform target;
    
    [Tooltip("Смещение камеры относительно игрока")]
    public Vector3 offset = new Vector3(0, 0, -10);
    
    [Tooltip("Скорость следования (0-1, где 1 = мгновенно)")]
    [Range(0.01f, 1f)]
    public float smoothSpeed = 0.125f;
    
    [Header("Ограничения движения")]
    [Tooltip("Использовать ограничения по осям")]
    public bool useBounds = false;
    
    [Tooltip("Минимальная позиция камеры")]
    public Vector3 minBounds = new Vector3(-10, -10, -10);
    
    [Tooltip("Максимальная позиция камеры")]
    public Vector3 maxBounds = new Vector3(10, 10, -10);
    
    void Start()
    {
        // Если цель не задана, ищем игрока автоматически
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("CameraFollow: Найден игрок автоматически");
            }
            else
            {
                Debug.LogWarning("CameraFollow: Игрок не найден! Назначьте Target в инспекторе или добавьте тег 'Player' игроку.");
            }
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Вычисляем желаемую позицию
        Vector3 desiredPosition = target.position + offset;
        
        // Применяем ограничения, если включены
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minBounds.z, maxBounds.z);
        }
        
        // Плавно перемещаем камеру
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
    
    // Визуализация границ в редакторе
    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = (minBounds + maxBounds) / 2;
            Vector3 size = maxBounds - minBounds;
            Gizmos.DrawWireCube(center, size);
        }
        
        // Показываем текущую цель
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position + offset);
            Gizmos.DrawWireSphere(target.position + offset, 0.5f);
        }
    }
}
