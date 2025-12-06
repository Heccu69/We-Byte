using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Korzh,      // Корж
    Cockroach,  // Таракан
    Spoon       // Ложка
}

public class PickupObject : MonoBehaviour
{
    public ObjectType objectType;
    public string objectName;
    
    [HideInInspector]
    public bool isPickedUp = false;
    
    private Rigidbody2D rb;
    private Collider2D col;
    private Transform originalParent;
    private Vector3 originalScale; // Исходный масштаб
    private Transform currentHandTransform; // Рука, к которой прикреплен
    
    private void Start()
    {
        // Устанавливаем имя объекта в зависимости от типа
        switch (objectType)
        {
            case ObjectType.Korzh:
                objectName = "Корж";
                break;
            case ObjectType.Cockroach:
                objectName = "Таракан";
                break;
            case ObjectType.Spoon:
                objectName = "Ложка";
                break;
        }
        
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        originalParent = transform.parent;
        originalScale = transform.localScale; // Сохраняем исходный масштаб
    }
    
    void LateUpdate()
    {
        // Если объект в руках, принудительно обновляем позицию
        if (isPickedUp && currentHandTransform != null)
        {
            transform.position = currentHandTransform.position;
            transform.rotation = currentHandTransform.rotation;
        }
    }
    
    public void Pickup(Transform handTransform)
    {
        if (isPickedUp) return;
        
        isPickedUp = true;
        currentHandTransform = handTransform;
        Debug.Log($"Подобран объект: {objectName}");
        
        // Отключаем коллайдер (чтобы не сталкиваться с игроком)
        if (col != null)
        {
            col.enabled = false;
        }
        
        // Отключаем физику в руках
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic; // Kinematic в руках
        }
        
        // Прикрепляем к руке игрока
        transform.SetParent(handTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        // Уменьшаем размер объекта в руках
        transform.localScale = Vector3.one * 0.5f;
    }
    
    public void Drop()
    {
        if (!isPickedUp) return;
        
        isPickedUp = false;
        currentHandTransform = null;
        Debug.Log($"Выброшен объект: {objectName}");
        
        // Отсоединяем от руки
        transform.SetParent(originalParent);
        transform.localScale = originalScale; // Восстанавливаем исходный масштаб
        
        // Включаем коллайдер обратно
        if (col != null)
        {
            col.enabled = true;
        }
        
        // Включаем физику обратно
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Dynamic после выбрасывания
            rb.gravityScale = 1f;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
