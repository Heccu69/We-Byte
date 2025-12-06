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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        // Анимация движения (если параметр существует)
        if (animator != null)
        {
            if (hor != 0 || ver != 0)
            {
                animator.SetBool("run", true);
            }
            else
            {
                animator.SetBool("run", false);
            }
        }
        // Анимация подбора (если параметр существует)
        if (animator != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetBool("Catch", true);
            }
            else
            {
                animator.SetBool("Catch", false);
            }
        }

        // Задаем скорость Rigidbody2D по обеим осям
        rgb.velocity = new Vector2(hor * speed, ver * speed);
    }
}
