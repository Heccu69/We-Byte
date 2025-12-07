using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    private float hor;
    private float ver;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D rgb;
    private PlayerCatch playerCatch;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCatch = GetComponent<PlayerCatch>();
        
        // Настраиваем физику игрока для толкания объектов
        if (rgb != null)
        {
            rgb.bodyType = RigidbodyType2D.Dynamic; // Dynamic для физических взаимодействий
            rgb.gravityScale = 0f; // Нет гравитации (вид сверху)
            rgb.mass = 3f; // Увеличенная масса для толкания коржей
            rgb.drag = 0f; // Нет сопротивления
            rgb.angularDrag = 0f;
            rgb.constraints = RigidbodyConstraints2D.FreezeRotation; // Не вращаться
        }
    }

    void Update()
    {
        // Получаем ввод по горизонтали и вертикали
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");

        // Поворот спрайта по горизонтали
        if (hor == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (hor == 1)
        {
            spriteRenderer.flipX = false;
        }
        
        // Анимация подбора - остается активной, пока игрок держит объект
        if (animator != null && playerCatch != null)
        {
            // Устанавливаем анимацию взятия, если игрок держит объект
            animator.SetBool("Catch", playerCatch.HasObject());
        }

        // Задаем скорость Rigidbody2D по обеим осям
        rgb.velocity = new Vector2(hor * speed, ver * speed);
    }
}
