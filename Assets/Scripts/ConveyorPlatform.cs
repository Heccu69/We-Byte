using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorPlatform : MonoBehaviour
{
    void Start()
    {
        // Настраиваем платформу как твердый объект
        SetupPlatform();
    }
    
    void SetupPlatform()
    {
        // Добавляем Rigidbody2D для физики
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Настраиваем как Kinematic - не падает, но твердый
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        
        // Добавляем коллайдер для столкновений
        BoxCollider2D col = gameObject.GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
        }
        
        // НЕ триггер - твердая поверхность
        col.isTrigger = false;
        
        // Настраиваем МАЛЕНЬКИЙ размер коллайдера (только верхняя поверхность)
        // Это позволит игроку подлетать снизу и брать коржи
        col.size = new Vector2(1f, 0.1f); // Тонкая платформа
        col.offset = new Vector2(0, 0); // По центру
        
        Debug.Log($"Платформа {gameObject.name} настроена как твердый объект");
    }
}
