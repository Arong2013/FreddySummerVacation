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
            transform.position = cur_move_pos_list[pos_index++].position;
            yield return new WaitForSeconds(move_delaying);
            //문을 닫았었는지 확인
        }
        //반복문 빠져나오면 플레이어 공격
    }
}
