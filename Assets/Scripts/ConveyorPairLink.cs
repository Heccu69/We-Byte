using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Связывает корж и платформу для одновременного удаления
/// </summary>
public class ConveyorPairLink : MonoBehaviour
{
    [Header("Связанный объект")]
    public GameObject pairedObject; // Связанный объект (платформа для коржа, корж для платформы)
    
    void OnDestroy()
    {
        // Если связь разорвана (null) - ничего не делаем
        if (pairedObject == null)
        {
            Debug.Log($"{gameObject.name} удален, связь уже разорвана");
            return;
        }
        
        // Проверяем, не подобран ли связанный объект
        PickupObject pickupObj = pairedObject.GetComponent<PickupObject>();
        if (pickupObj == null || !pickupObj.isPickedUp)
        {
            // Удаляем связанный объект
            Destroy(pairedObject);
            Debug.Log($"{gameObject.name} удален, удаляем связанный {pairedObject.name}");
        }
        else
        {
            // Корж подобран - не удаляем
            Debug.Log($"{gameObject.name} удален, но {pairedObject.name} подобран - не удаляем");
        }
    }
}
