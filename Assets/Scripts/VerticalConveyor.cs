using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalConveyor : MonoBehaviour
{
    [Header("Настройки конвейера")]
    public float conveyorSpeed = 2f; // Скорость движения конвейера
    public bool moveUp = true; // Направление движения (вверх или вниз)
    
    [Header("Границы конвейера")]
    public float topBound = 5f; // Верхняя граница (относительно позиции конвейера)
    public float bottomBound = -5f; // Нижняя граница (относительно позиции конвейера)
    
    private List<Transform> objectsOnConveyor = new List<Transform>();
    
    void Update()
    {
        // Двигаем все объекты на конвейере
        MoveObjectsOnConveyor();
    }
    
    void MoveObjectsOnConveyor()
    {
        // Определяем направление движения
        float direction = moveUp ? 1f : -1f;
        Vector2 velocity = new Vector2(0, direction * conveyorSpeed);
        
        // Двигаем все объекты на конвейере
        for (int i = objectsOnConveyor.Count - 1; i >= 0; i--)
        {
            if (objectsOnConveyor[i] == null)
            {
                objectsOnConveyor.RemoveAt(i);
                continue;
            }
            
            Transform obj = objectsOnConveyor[i];
            
            // Проверяем, не подобран ли объект
            PickupObject pickupObj = obj.GetComponent<PickupObject>();
            if (pickupObj != null && pickupObj.isPickedUp)
            {
                objectsOnConveyor.RemoveAt(i);
                continue;
            }
            
            // Двигаем через Rigidbody2D если есть
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = velocity;
            }
            else
            {
                obj.position += (Vector3)(velocity * Time.deltaTime);
            }
            
            // Проверяем границы в мировых координатах
            float worldTopBound = transform.position.y + topBound;
            float worldBottomBound = transform.position.y + bottomBound;
            
            if (moveUp && obj.position.y > worldTopBound)
            {
                Debug.Log($"Объект {obj.name} удален за верхней границей");
                Destroy(obj.gameObject);
                objectsOnConveyor.RemoveAt(i);
            }
            else if (!moveUp && obj.position.y < worldBottomBound)
            {
                Debug.Log($"Объект {obj.name} удален за нижней границей");
                Destroy(obj.gameObject);
                objectsOnConveyor.RemoveAt(i);
            }
        }
    }
    
    // Когда объект попадает на конвейер
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ConveyorObject") || collision.GetComponent<PickupObject>() != null)
        {
            if (!objectsOnConveyor.Contains(collision.transform))
            {
                objectsOnConveyor.Add(collision.transform);
                
                // Отключаем гравитацию на конвейере
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = 0f;
                }
                
                Debug.Log($"Объект {collision.name} попал на конвейер");
            }
        }
    }
    
    // Когда объект покидает конвейер
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (objectsOnConveyor.Contains(collision.transform))
        {
            objectsOnConveyor.Remove(collision.transform);
            
            // Включаем гравитацию обратно
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f;
            }
            
            Debug.Log($"Объект {collision.name} покинул конвейер");
        }
    }
    
    // Визуализация границ конвейера в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        
        // Рисуем верхнюю границу
        Vector3 topLeft = transform.position + new Vector3(-0.5f, topBound, 0);
        Vector3 topRight = transform.position + new Vector3(0.5f, topBound, 0);
        Gizmos.DrawLine(topLeft, topRight);
        
        // Рисуем нижнюю границу
        Vector3 bottomLeft = transform.position + new Vector3(-0.5f, bottomBound, 0);
        Vector3 bottomRight = transform.position + new Vector3(0.5f, bottomBound, 0);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        
        // Рисуем боковые линии
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
        
        // Стрелка направления
        Vector3 arrowStart = transform.position;
        Vector3 arrowEnd = transform.position + new Vector3(0, moveUp ? 1f : -1f, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(arrowStart, arrowEnd);
    }
}
