using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain : MonoBehaviour
{
    bool isAttack = false;
    float move_delaying = 1.0f;  ///////테스트용 1초 나중에 기본값 15
    float defeatTime = 3.0f; ///////테스트용 나중에 수정 기본값 15
    int pos_index = 0;
    float attackSpeed = 10f;
    [SerializeField] Door door;
    [SerializeField] Transform[] move_Pos;//모든방을 넣어놓고 이동순서를 따로 배열로 저장 
    [SerializeField] int[] move_index;//
    [SerializeField] Transform door_pos;
    [SerializeField] Player player;
    Coroutine move_coroutine;
    
    public bool IsAttack {  set {isAttack = value;} get {return isAttack;}    }
    public void Initialize()
    {
        pos_index = 0;
        isAttack = false;
        //SetDifficulty(VILLAIN_DIFFICULTY.NORMAL);
        move_coroutine = StartCoroutine(Move());
    }
    public void SetDifficulty(VILLAIN_DIFFICULTY difficulty)
    {
        switch(difficulty)
        {
            case VILLAIN_DIFFICULTY.EASY:
                move_delaying = 15.0f;//나중에 난이도별로 수정
                defeatTime = 25.0f;
                break;
            case VILLAIN_DIFFICULTY.NORMAL:
                move_delaying = 15.0f;
                defeatTime = 15.0f;
                break;
            case VILLAIN_DIFFICULTY.HARD:
                move_delaying = 15.0f;
                defeatTime = 10.0f;
                break;
        }
    }
    IEnumerator Move()
    {
        while(!isAttack)
        {
            if(pos_index >= move_Pos.Length) pos_index = 0;
            transform.position = move_Pos[pos_index++].position;
            yield return new WaitForSeconds(move_delaying);
        }
        //반복문 빠져나오면 플레이어 공격
    }
    public IEnumerator CheckTime()
    {
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
        }
    }
    void AttackPlayer()
    {
        gameObject.transform.position = door_pos.position;
        door.OpenDoor();
    }
}
