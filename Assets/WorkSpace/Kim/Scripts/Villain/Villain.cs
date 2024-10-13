using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Villain : MonoBehaviour
{
    protected bool isAttack = false;
    protected float move_delaying;
    protected int pos_index = 0;
    protected float attackSpeed = 10f;
    [SerializeField] protected Door door;
    [SerializeField] protected Transform[] move_Pos_list;
    [SerializeField] protected Transform[] hard_move_Pos_list;
    [SerializeField] protected int return_index;
    [SerializeField] protected int hard_return_index;
    [SerializeField] protected Transform door_pos;
    [SerializeField] protected Player player;
    [SerializeField] protected AudioClip jumpSquare_SFX = null;
    [SerializeField] protected AudioClip walking_SFX = null;
    [SerializeField] protected RectTransform jump_square_image_RectTransform = null;
    [SerializeField] Vector2 attack_motion_move_startPos;
    [SerializeField] Vector2 attack_motion_move_targetPos;
    protected Coroutine move_coroutine;
    protected Transform[] cur_move_pos_list;
    protected int cur_return_index;
    protected bool isWaring = false;
    protected bool isClosing = false;
    public float attack_motion_move_animationDuration = 1.0f;  // 애니메이션 지속 시간

    private float attack_motion_move_animationTime = 0f;
    public bool IsAttack {  set {isAttack = value;} get {return isAttack;}    }
    public bool IsWaring {  set {isWaring = value;} get {return isWaring;}    }
    public bool IsClosing {  set {isClosing = value;} get {return isClosing;}    }
    public float GetMoveDelay => move_delaying;
    bool isGameStop = false;
    public bool IsGameStop{ set { isGameStop = value; } get {return isGameStop;}}
    //문을 닫았는지 확인할거 필요함
    public virtual void Initialize(VILLAIN_DIFFICULTY difficulty = VILLAIN_DIFFICULTY.NORMAL)
    {
        gameObject.SetActive(false);
        pos_index = 0;
        isAttack = false;
        isWaring = false;
        isClosing = false;
        SetDifficulty(difficulty);
    }
    public void StartMove()
    {
        gameObject.SetActive(true);
        move_coroutine = StartCoroutine(Move());
    }
    public void Stop()
    {
        if(move_coroutine != null)
            StopCoroutine(move_coroutine);
        gameObject.SetActive(false);
    }
    public virtual void SetDifficulty(VILLAIN_DIFFICULTY difficulty)
    {
        switch(difficulty)
        {
            case VILLAIN_DIFFICULTY.EASY:
                move_delaying = 20.0f;
                cur_move_pos_list = move_Pos_list;
                cur_return_index = return_index;
                break;
            case VILLAIN_DIFFICULTY.NORMAL:
                move_delaying = 15.0f;
                cur_move_pos_list = move_Pos_list;
                cur_return_index = return_index;
                break;
            case VILLAIN_DIFFICULTY.HARD:
                move_delaying = 10.0f;
                cur_move_pos_list = hard_move_Pos_list;
                cur_return_index = hard_return_index;
                break;
        }
    }
    public virtual IEnumerator Move()
    {
        yield break;
    }
    public virtual IEnumerator CheckTime()
    {
/*         if(!gameObject.activeSelf) yield break;
        // 시작 시간을 기록
        float startTime = Time.time;

        while (true)
        {
            // 현재 시간을 가져옴
            float elapsedTime = Time.time - startTime;

            // 경과 시간을 로그로 출력
            Debug.Log("Elapsed Time: " + elapsedTime + " seconds");

            // 1초 동안 대기
            yield return new WaitForSeconds(1f);

            // 15초가 지났는지 확인
            if (elapsedTime >= defeatTime)
            {
                Debug.Log("15초 지남 게임오버");
                isAttack = true;
                StopCoroutine(move_coroutine);
                AttackPlayer();
                // 15초가 지나면 반복을 종료
                break;
            }
        } */
        yield break;
    }
    protected virtual IEnumerator AttackPlayer()
    {
        isAttack = true;
        gameObject.transform.position = door_pos.position;
        gameObject.transform.rotation = door_pos.rotation;
        door.Forced_OpenDoor();
        Game_Manager.Instance.SetInputAction(false);
        yield return new WaitForSeconds(2.0f);////문앞에서 플레이어 공격하기까지의 딜레이
        transform.rotation = cur_move_pos_list[0].rotation;
        transform.position = cur_move_pos_list[0].position;//오브젝트를 끄면 밑의 코드가 작동이 안됨

        //직접적인 공격
        if(jumpSquare_SFX != null)
            Sound_Manager.Instance.PlaySFX(jumpSquare_SFX, (int)SFX_SOURCE_INDEX.NORMAL_SFX);
        jump_square_image_RectTransform.gameObject.SetActive(true);
        jump_square_image_RectTransform.anchoredPosition = attack_motion_move_targetPos;
        
        //StartCoroutine(Attack_Motion());//애니메이션 적용안됨
        yield return new WaitForSeconds(2.0f);
        Game_Manager.Instance.GameEnd(false);
        Debug.Log("플레이어 공격 성공");
    }

    /* protected IEnumerator Attack_Motion()
    {
        if(jumpSquare_SFX != null)
            Sound_Manager.Instance.PlaySFX(jumpSquare_SFX, (int)SFX_SOURCE_INDEX.NORMAL_SFX);

        float elapsedTime = 0f;
        jump_square_image_RectTransform.gameObject.SetActive(true);

        while (elapsedTime < attack_motion_move_animationDuration)
        {
            // 시간에 따라 위치를 선형적으로 이동
            jump_square_image_RectTransform.anchoredPosition = Vector2.Lerp(attack_motion_move_targetPos, attack_motion_move_targetPos, elapsedTime / attack_motion_move_animationDuration);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 한 프레임 대기
            yield return null;
        }

        // 최종 위치로 설정 (끝 위치)
        jump_square_image_RectTransform.anchoredPosition = attack_motion_move_targetPos;
    } */
}
