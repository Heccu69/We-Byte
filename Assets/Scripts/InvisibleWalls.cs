using UnityEngine;

public class InvisibleWalls : MonoBehaviour
{
    [Header("Размеры карты")]
    [Tooltip("Размер карты по горизонтали (ширина)")]
    public float mapWidth = 20f;
    
    [Tooltip("Размер карты по вертикали (высота)")]
    public float mapHeight = 15f;
    
    [Header("Настройки стен")]
    [Tooltip("Толщина невидимых стен")]
    public float wallThickness = 1f;
    
    [Tooltip("Слой для стен (например, 'Walls')")]
    public string wallLayer = "Default";
    
    void Start()
    {
        CreateInvisibleWalls();
    }
    
    void CreateInvisibleWalls()
    {
        // Создаем родительский объект для всех стен
        GameObject wallsParent = new GameObject("InvisibleWalls");
        wallsParent.transform.position = transform.position;
        
        // Верхняя стена
        CreateWall("TopWall", 
            new Vector2(0, mapHeight / 2 + wallThickness / 2), 
            new Vector2(mapWidth + wallThickness * 2, wallThickness), 
            wallsParent.transform);
        
        // Нижняя стена
        CreateWall("BottomWall", 
            new Vector2(0, -mapHeight / 2 - wallThickness / 2), 
            new Vector2(mapWidth + wallThickness * 2, wallThickness), 
            wallsParent.transform);
        
        // Левая стена
        CreateWall("LeftWall", 
            new Vector2(-mapWidth / 2 - wallThickness / 2, 0), 
            new Vector2(wallThickness, mapHeight), 
            wallsParent.transform);
        
        // Правая стена
        CreateWall("RightWall", 
            new Vector2(mapWidth / 2 + wallThickness / 2, 0), 
            new Vector2(wallThickness, mapHeight), 
            wallsParent.transform);
        
        Debug.Log($"Невидимые стены созданы вокруг карты {mapWidth}x{mapHeight}");
    }
    
    void CreateWall(string name, Vector2 position, Vector2 size, Transform parent)
    {
        GameObject wall = new GameObject(name);
        wall.transform.SetParent(parent);
        wall.transform.localPosition = position;
        
        // Добавляем BoxCollider2D
        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = false; // Физическая стена, не триггер
        
        // Устанавливаем слой
        wall.layer = LayerMask.NameToLayer(wallLayer);
    }
    
    // Визуализация границ в редакторе
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Vector3 center = transform.position;
        
        // Верхняя граница
        Gizmos.DrawLine(
            center + new Vector3(-mapWidth / 2, mapHeight / 2, 0),
            center + new Vector3(mapWidth / 2, mapHeight / 2, 0)
        );
        
        // Нижняя граница
        Gizmos.DrawLine(
            center + new Vector3(-mapWidth / 2, -mapHeight / 2, 0),
            center + new Vector3(mapWidth / 2, -mapHeight / 2, 0)
        );
        
        // Левая граница
        Gizmos.DrawLine(
            center + new Vector3(-mapWidth / 2, -mapHeight / 2, 0),
            center + new Vector3(-mapWidth / 2, mapHeight / 2, 0)
        );
        
        // Правая граница
        Gizmos.DrawLine(
            center + new Vector3(mapWidth / 2, -mapHeight / 2, 0),
            center + new Vector3(mapWidth / 2, mapHeight / 2, 0)
        );
    }
}
