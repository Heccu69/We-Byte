using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatch : MonoBehaviour
{
    [Header("Настройки подбора")]
    public float pickupRange = 1.5f; // Радиус подбора объектов
    public LayerMask pickupLayer; // Слой для объектов, которые можно подобрать
    
    [Header("Точка прикрепления")]
    public Transform handTransform; // Точка прикрепления объекта (рука)
    public Vector3 handOffset = new Vector3(0.5f, 0.3f, 0f); // Смещение руки относительно игрока
    
    private PickupObject heldObject = null; // Объект в руке (только один)
    private Transform actualHandTransform; // Реальная точка руки
    
    void Start()
    {
        // Создаем точку руки, если не задана
        if (handTransform == null)
        {
            GameObject handObj = new GameObject("Hand");
            actualHandTransform = handObj.transform;
            actualHandTransform.SetParent(transform);
            actualHandTransform.localPosition = handOffset;
        }
        else
        {
            actualHandTransform = handTransform;
        }
    }
    
    void Update()
    {
        // Проверяем нажатие клавиши E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickupObject();
            }
            else
            {
                DropObject();
            }
        }
    }
    
    void LateUpdate()
    {
        // Обновляем позицию руки ПОСЛЕ движения игрока
        if (actualHandTransform != null)
        {
            // Если рука создана автоматически, обновляем ее позицию
            if (handTransform == null)
            {
                actualHandTransform.localPosition = handOffset;
            }
        }
    }
    
    void TryPickupObject()
    {
        // Ищем все объекты в радиусе подбора
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange, pickupLayer);
        
        PickupObject closestObject = null;
        float closestDistance = float.MaxValue;
        
        // Находим ближайший объект
        foreach (Collider2D col in colliders)
        {
            PickupObject pickupObj = col.GetComponent<PickupObject>();
            
            if (pickupObj != null && !pickupObj.isPickedUp)
            {
                float distance = Vector2.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = pickupObj;
                }
            }
        }
        
        // Подбираем ближайший объект
        if (closestObject != null)
        {
            heldObject = closestObject;
            Debug.Log($"Подобран объект: {closestObject.objectName}");
            
            // Вызываем метод подбора объекта
            closestObject.Pickup(actualHandTransform);
        }
    }
    
    void DropObject()
    {
        if (heldObject == null) return;
        
        Debug.Log($"Выброшен объект: {heldObject.objectName}");
        
        // Выбрасываем объект
        heldObject.Drop();
        heldObject = null;
    }
    
    // Проверка, есть ли объект в руке
    public bool HasObject()
    {
        return heldObject != null;
    }
    
    // Получить тип объекта в руке
    public ObjectType? GetHeldObjectType()
    {
        if (heldObject != null)
        {
            return heldObject.objectType;
        }
        return null;
    }
    
    // Визуализация радиуса подбора и руки в редакторе
    private void OnDrawGizmosSelected()
    {
        // Радиус подбора
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
        
        // Позиция руки
        Gizmos.color = Color.green;
        Vector3 handPos = transform.position + handOffset;
        Gizmos.DrawWireCube(handPos, Vector3.one * 0.2f);
        Gizmos.DrawLine(transform.position, handPos);
    }
}
