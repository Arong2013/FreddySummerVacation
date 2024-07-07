using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    public UnityEvent<Collider2D> OnTriggerEnter2DEvent;
    Rigidbody2D RB;
    Animator AN;
    SpriteRenderer SR;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        AN = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 이동
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        if (movement != Vector2.zero)
        {
            RB.velocity = movement * 4;
            SR.flipX = moveHorizontal == -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnter2DEvent?.Invoke(other);
    }
}
