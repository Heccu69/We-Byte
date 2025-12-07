using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Rigidbody2D rgb;
    public SpriteRenderer spriteRenderer;
    public float _speed = 2f; // Скорость движения
    
    [Header("Настройки движения")]
    public float changeDirectionTime = 2f; // Время до смены направления
    public Vector2 movementAreaMin = new Vector2(-10f, -5f); // Минимальные границы области
    public Vector2 movementAreaMax = new Vector2(10f, 5f);   // Максимальные границы области
    
    [Header("Настройки испуга")]
    public float scaredSpeedMultiplier = 2f; // Множитель скорости при испуге
    
    private Vector2 currentDirection;
    private float directionTimer;
    
    // Состояние испуга
    private bool isScared = false;
    private float scaredTimer = 0f;

    private void Start()
    {
        // Настраиваем физику для движения по столу без прыжков
        if (rgb != null)
        {
            rgb.gravityScale = 3f; // Увеличенная гравитация
            
            // Замораживаем вращение, чтобы таракан не переворачивался
            rgb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            // Отключаем возможность прыжков (таракан остается на поверхности)
            rgb.mass = 1f;
            rgb.drag = 0.5f; // Небольшое сопротивление
        }
        
        // Выбираем случайное направление (только по горизонтали)
        ChooseRandomDirection();
        
        // Сбрасываем таймер
        directionTimer = changeDirectionTime;
    }
    
    /// <summary>
    /// Выбрать случайное направление движения (только горизонтальное)
    /// </summary>
    void ChooseRandomDirection()
    {
        // Случайное направление: влево (-1) или вправо (1)
        float horizontalDir = Random.value > 0.5f ? 1f : -1f;
        
        // Только горизонтальное движение (Y = 0), чтобы не прыгать
        currentDirection = new Vector2(horizontalDir, 0f);
    }

    private void Update()
    {
        // Обновляем таймер испуга
        if (isScared)
        {
            scaredTimer -= Time.deltaTime;
            if (scaredTimer <= 0f)
            {
                isScared = false;
            }
        }
        
        // Уменьшаем таймер смены направления (только если не испуган)
        if (!isScared)
        {
            directionTimer -= Time.deltaTime;
            
            // Если время вышло - выбираем новое направление
            if (directionTimer <= 0f)
            {
                ChooseRandomDirection();
                directionTimer = changeDirectionTime;
            }
        }
        
        // Проверяем границы и отражаем направление при столкновении
        Vector2 currentPos = transform.position;
        
        if (currentPos.x <= movementAreaMin.x || currentPos.x >= movementAreaMax.x)
        {
            currentDirection.x = -currentDirection.x; // Отражаем по X
        }
        
        if (currentPos.y <= movementAreaMin.y || currentPos.y >= movementAreaMax.y)
        {
            currentDirection.y = -currentDirection.y; // Отражаем по Y
        }
        
        // Вычисляем скорость (увеличенная при испуге)
        float currentSpeed = isScared ? _speed * scaredSpeedMultiplier : _speed;
        
        // Двигаемся в текущем направлении
        if (rgb != null)
        {
            rgb.velocity = currentDirection * currentSpeed;
        }
        
        // Поворот спрайта в сторону движения
        if (spriteRenderer != null)
        {
            if (currentDirection.x < 0)
            {
                spriteRenderer.flipX = false; // Смотрит влево
            }
            else if (currentDirection.x > 0)
            {
                spriteRenderer.flipX = true; // Смотрит вправо
            }
        }
    }
    
    /// <summary>
    /// Испугать таракана (вызывается ложкой)
    /// </summary>
    public void GetScared(float duration)
    {
        isScared = true;
        scaredTimer = duration;
        
        // Таракан бежит быстрее, когда испуган
        Debug.Log($"Таракан {gameObject.name} испуган!");
    }
}
