using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    private float hor;
    private float ver;

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

        // Задаем скорость Rigidbody2D по обеим осям
        rgb.velocity = new Vector2(hor * speed, ver * speed);
    }
}
