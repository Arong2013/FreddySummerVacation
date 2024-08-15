using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain_B : Villain
{
    [SerializeField] int second_return_index;
    [SerializeField] int second_hard_return_index;
    int second_cur_return_index;
    public override void SetDifficulty(VILLAIN_DIFFICULTY difficulty)
    {
        switch(difficulty)
        {
            case VILLAIN_DIFFICULTY.EASY:
                move_delaying = 20.0f;
                cur_move_pos_list = move_Pos_list;
                cur_return_index = return_index;
                second_cur_return_index = second_return_index;
                break;
            case VILLAIN_DIFFICULTY.NORMAL:
                move_delaying = 15.0f;
                cur_move_pos_list = move_Pos_list;
                cur_return_index = return_index;
                second_cur_return_index = second_return_index;
                break;
            case VILLAIN_DIFFICULTY.HARD:
                move_delaying = 10.0f;
                cur_move_pos_list = hard_move_Pos_list;
                cur_return_index = hard_return_index;
                second_cur_return_index = second_hard_return_index;
                break;
        }
    }
    public override IEnumerator Move()
    {
        while(!isAttack)
        {
            if((cur_return_index == 0 && pos_index > 2) || (cur_return_index != 0 && pos_index >= cur_move_pos_list.Length - 1)) 
                pos_index = cur_return_index;
            transform.position = cur_move_pos_list[pos_index].position;
            yield return new WaitForSeconds(move_delaying);
            if(isWaring && cur_move_pos_list[pos_index].gameObject.name == "BreakRoom")//탕비실에서 다음위치로 이동할때까지 경보음을 울렸으면 식칼챙김
            {
                Debug.Log("식칼 챙김");
                cur_return_index = second_cur_return_index;
            }
            else if(!isClosing &&cur_move_pos_list[pos_index].gameObject.name == "Door_Pos")//철문앞에서 다음위치로 이동할때까지 문을 닫지않으면 플레이어 공격
            {
                Debug.Log("플레이어 공격");
                AttackPlayer();
            }
            pos_index++;
            isClosing = false;
            isWaring = false;
        }
        //반복문 빠져나오면 플레이어 공격
    }
}
