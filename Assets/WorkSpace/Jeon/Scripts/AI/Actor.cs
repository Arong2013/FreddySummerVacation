using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    Rigidbody2D RB;
    Animator AN;
    SpriteRenderer SR;
    Dictionary<KeyCode, Action> keyActions = new Dictionary<KeyCode, Action>();

    private Vector2 lastMovement = Vector2.down; // 기본 방향을 아래로 설정
    private bool isMoving = false;

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
            lastMovement = movement;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        RB.velocity = movement * 4;

        // 애니메이션 및 스프라이트 방향 설정
        UpdateAnimationAndDirection();

        foreach (var keyAction in keyActions)
        {
            if (Input.GetKeyDown(keyAction.Key))
            {
                keyAction.Value.Invoke();
            }
        }
    }

    private void UpdateAnimationAndDirection()
    {
        // 움직임 방향 설정
        AN.SetFloat("WalkX", lastMovement.x);
        AN.SetFloat("WalkY", lastMovement.y);

        // 스프라이트 방향 설정
        if (lastMovement.x != 0)
        {
            SR.flipX = lastMovement.x < 0;
        }

        // 애니메이션 재생 여부 설정
        if (isMoving)
        {
            AN.speed = 1; // 애니메이션 재생
        }
        else
        {
            AN.speed = 0; // 애니메이션 정지
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<DayDoor>(out DayDoor component))
        {
            keyActions.Add(KeyCode.Space, component.OpenDoor);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Door>(out Door component))
        {
            keyActions.Remove(KeyCode.Space);
        }
    }
}