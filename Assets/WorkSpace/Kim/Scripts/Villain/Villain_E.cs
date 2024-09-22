using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain_E : Villain
{
    float defeatTime = 15.0f;
    bool attackable = false;
    bool attacking = false;
    public override void SetDifficulty(VILLAIN_DIFFICULTY difficulty)
    {
        switch(difficulty)
        {
            case VILLAIN_DIFFICULTY.EASY:
                move_delaying = 20.0f;//나중에 난이도별로 수정
                defeatTime = 20.0f;
                cur_move_pos_list = move_Pos_list;
                break;
            case VILLAIN_DIFFICULTY.NORMAL:
                move_delaying = 15.0f;
                defeatTime = 15.0f;
                cur_move_pos_list = move_Pos_list;
                break;
            case VILLAIN_DIFFICULTY.HARD:
                move_delaying = 10.0f;
                defeatTime = 10.0f;
                cur_move_pos_list = hard_move_Pos_list;
                break;
        }
    }
    public override IEnumerator CheckTime()
    {
        if(!gameObject.activeSelf || !attackable) yield break;
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
                attacking = true;
                // 15초가 지나면 반복을 종료
                break;
            }
        }
    }
    public override IEnumerator Move()
    {
        while(!isAttack)
        {
            if(pos_index >= cur_move_pos_list.Length) pos_index = cur_return_index;
            transform.rotation = cur_move_pos_list[pos_index].rotation;
            transform.position = cur_move_pos_list[pos_index].position;
            if(cur_move_pos_list[pos_index].gameObject.name == "Lobby")//문을 닫는지는 위의 CheckTime함수로 체크
                attackable = true;
            yield return new WaitForSeconds(move_delaying);
            attackable = false;
            pos_index++;
            isClosing = false;
            isWaring = false;
        }
        //반복문 빠져나오면 플레이어 공격
    }
}
