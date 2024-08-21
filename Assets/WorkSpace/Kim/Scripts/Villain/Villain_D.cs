using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain_D : Villain
{
    public override IEnumerator Move()
    {
        while(!isAttack)
        {
            if(pos_index >= cur_move_pos_list.Length) pos_index = cur_return_index;
            transform.position = cur_move_pos_list[pos_index].position;
            yield return new WaitForSeconds(move_delaying);
            //문을 닫았었는지 확인
            if(!isClosing && cur_move_pos_list[pos_index].gameObject.name == "Door_Pos")//철문앞에서 다음위치로 이동할때까지 문을 닫지않으면 플레이어 공격
            {//마지막 위치가 철문
                Debug.Log("플레이어 공격");
                AttackPlayer();
            }
            else if(cur_move_pos_list[pos_index].gameObject.name == "None")
            {
                SoundManager.Instance.PlaySound(SOUND_INDEX.NEIGHBOR_NOISE);//사라져서 층간소음내기 시작하고 다음위치로 갔을때 소음멈추기
            }
            pos_index++;
            isClosing = false;
            isWaring = false;
        }
        //반복문 빠져나오면 플레이어 공격
    }
}
