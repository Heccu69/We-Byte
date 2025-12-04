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
    public float stopDistance = 0.5f;

    private void Update()
    {
        // Вычисляем вектор направления к торту
        Vector2 direction = (cake.position - transform.position).normalized;
        
        // Вычисляем расстояние до торта
        float distanceToCake = Vector2.Distance(transform.position, cake.position);
        
        
        if (distanceToCake > stopDistance)
        {
            // Если дальше порога — двигаемся к торту
            rgb.velocity = direction * _speed;
        }
        else
        {
            // Если близко — останавливаемся
            rgb.velocity = Vector2.zero;
        }

        // Поворот спрайта в сторону торта
        if (cake.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
