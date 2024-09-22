using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain_C : Villain
{
    public override IEnumerator Move()
    {
        while(!isAttack)
        {
            if(pos_index >= cur_move_pos_list.Length) pos_index = cur_return_index;
            transform.rotation = cur_move_pos_list[pos_index].rotation;
            transform.position = cur_move_pos_list[pos_index].position;
            yield return new WaitForSeconds(move_delaying);
            if(!isClosing && cur_move_pos_list[pos_index].gameObject.name == "Door_Pos")//철문앞에서 다음위치로 이동할때까지 문을 닫지않으면 플레이어 공격
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
