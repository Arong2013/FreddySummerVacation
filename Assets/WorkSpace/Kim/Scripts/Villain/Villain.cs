using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain : MonoBehaviour
{
    protected bool isAttack = false;
    protected float move_delaying = 15.0f;
    protected int pos_index = 0;
    protected float attackSpeed = 10f;
    [SerializeField] protected Door door;
    [SerializeField] protected Transform[] move_Pos_list;
    [SerializeField] protected Transform[] hard_move_Pos_list;
    [SerializeField] protected int return_index;
    [SerializeField] protected int hard_return_index;
    [SerializeField] protected Transform door_pos;
    [SerializeField] protected Player player;
    [SerializeField] AudioClip jumpSquare_SFX;
    protected Coroutine move_coroutine;
    protected Transform[] cur_move_pos_list;
    protected int cur_return_index;
    protected bool isWaring = false;
    protected bool isClosing = false;
    public bool IsAttack {  set {isAttack = value;} get {return isAttack;}    }
    public bool IsWaring {  set {isWaring = value;} get {return isWaring;}    }
    public bool IsClosing {  set {isClosing = value;} get {return isClosing;}    }
    public float GetMoveDelay => move_delaying;
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
        while(!isAttack)
        {
            if(pos_index >= cur_move_pos_list.Length) pos_index = cur_return_index;
            transform.rotation = cur_move_pos_list[pos_index].rotation;
            transform.position = cur_move_pos_list[pos_index].position;
            yield return new WaitForSeconds(move_delaying);
            isClosing = false;
            isWaring = false;
            pos_index++;
        }
        //반복문 빠져나오면 플레이어 공격
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
        StopCoroutine(move_coroutine);
        gameObject.transform.position = door_pos.position;
        door.OpenDoor();
        yield return new WaitForSeconds(3f);////문앞에서 플레이어 공격하기까지의 딜레이

        if(jumpSquare_SFX != null)
            Sound_Manager.Instance.PlaySFX(jumpSquare_SFX, (int)SFX_SOURCE_INDEX.NORMAL_SFX);
            
        //직접적인 공격
        
    }
}
