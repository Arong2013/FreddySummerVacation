using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System.Runtime.Serialization;  // Odin Inspector 네임스페이스 추가
public class Actor : SerializedMonoBehaviour
{
    Rigidbody2D RB;
    Animator AN;
    SpriteRenderer SR;
    Dictionary<KeyCode, Action> keyActions = new Dictionary<KeyCode, Action>();

    private Vector2 lastMovement = Vector2.down; // 기본 방향을 아래로 설정
    private bool isMoving = false;
    private string lastAnimation = "Walk_Down"; // 마지막으로 재생된 애니메이션 이름 저장

    [SerializeField, OdinSerialize]
    Dictionary<string, Sprite> directionSprites = new Dictionary<string, Sprite>();

    [SerializeField] AudioClip audioClip, walk;
    [SerializeField] private float movementSpeed = 4f; // 이동 속도 인스펙터에서 설정 가능

    private float walkSoundCooldown = 0.5f; // 걷는 소리가 재생되는 간격 (0.5초)
    private float lastWalkSoundTime = 0f;   // 마지막 걷는 소리가 재생된 시간

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        AN = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();

        // BGM 재생
        Sound_Manager.Instance.PlayBGM(audioClip);
    }

    private void Update()
    {
        // UI가 활성화되어 있지 않을 때만 캐릭터가 움직일 수 있게 설정
        if (!UiUtils.GetUI<DialogueManager>().gameObject.activeSelf)
        {
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

            RB.velocity = movement * movementSpeed;
        }
        else
        {
            // 대화가 시작되면 lastAnimation을 "Walk_Up"으로 강제 설정
            lastAnimation = "Walk_Up";
            isMoving = false;
        }

        UpdateAnimationAndDirection();
    }


    private void UpdateAnimationAndDirection()
    {
        // 스프라이트 방향 설정 (좌우 반전)
        if (lastMovement.x != 0)
        {
            SR.flipX = lastMovement.x < 0;  // x 값이 음수면 스프라이트 좌우 반전
        }

        if (isMoving)
        {
            AN.speed = 1;
            // 걷는 방향에 따라 애니메이션 실행
            if (lastMovement.y > 0)
            {
                lastAnimation = "Walk_Up";  // 위쪽 걷기 애니메이션
                AN.Play(lastAnimation);
            }
            else if (lastMovement.y < 0)
            {
                lastAnimation = "Walk_Down";  // 아래쪽 걷기 애니메이션
                AN.Play(lastAnimation);
            }
            else if (lastMovement.x != 0)  // 좌우 이동일 경우
            {
                lastAnimation = "Walk_Side";  // 좌우 걷기 애니메이션 (하나의 애니메이션만 사용)
                AN.Play(lastAnimation);
            }

            // 0.5초 간격으로 걷는 소리 재생
            if (Time.time - lastWalkSoundTime >= walkSoundCooldown && !Sound_Manager.Instance.IsPlayingAudioSource(walk))
            {
                Sound_Manager.Instance.PlaySFX(walk); // 걷는 소리 재생
                lastWalkSoundTime = Time.time; // 마지막 걷는 소리 재생 시간 업데이트
            }
        }
        else
        {
            UpdateSpriteDirection(lastAnimation);
            AN.speed = 0; // 애니메이션을 멈추고 해당 프레임에 고정
        }
    }

    private void UpdateSpriteDirection(string lastAnime)
    {
        // lastDirection에 해당하는 스프라이트가 있으면 스프라이트를 변경
        if (directionSprites.ContainsKey(lastAnime))
        {
            SR.sprite = directionSprites[lastAnime];
        }
    }
}
