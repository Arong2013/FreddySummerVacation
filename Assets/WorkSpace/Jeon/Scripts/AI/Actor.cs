using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.Serialization;
using Sirenix.OdinInspector;  // Odin Inspector 네임스페이스 추가

public class Actor : SerializedMonoBehaviour
{
    Rigidbody2D RB;
    Animator AN;
    SpriteRenderer SR;
    Dictionary<KeyCode, Action> keyActions = new Dictionary<KeyCode, Action>();

    private Vector2 lastMovement = Vector2.down; // 기본 방향을 아래로 설정
    private bool isMoving = false;

    // 인스펙터에서 설정할 방향과 스프라이트 딕셔너리
    [SerializeField, OdinSerialize]
    private Dictionary<Vector2, Sprite> directionSprites = new Dictionary<Vector2, Sprite>();

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
        AN.SetBool("IsWalk", isMoving);

        if (isMoving)
        {
            AN.speed = 1; // 애니메이션 재생
        }
        else
        {
            AN.speed = 0; // 애니메이션 정지

            // 걷지 않을 때 스프라이트 변경
            UpdateSpriteDirection();
        }
    }

    private void UpdateSpriteDirection()
    {
        if (directionSprites.ContainsKey(lastMovement))
        {
            SR.sprite = directionSprites[lastMovement];
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
