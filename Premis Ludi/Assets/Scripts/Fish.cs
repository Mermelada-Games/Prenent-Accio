using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float distance = 1.0f;

    private SpriteRenderer spriteRenderer;
    private float initialPosition;
    private int direction = 1;
    public bool isHooked = false;

    private void Start()
    {
        initialPosition = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isHooked)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        if (transform.position.x <= initialPosition - distance)
        {
            direction = 1;
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x >= initialPosition + distance)
        {
            direction = -1;
            spriteRenderer.flipX = true;
        }
    }
}