using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain_A : Villain
{
    public override IEnumerator Move()
    {
        while(true)
        {
            if(pos_index >= cur_move_pos_list.Length) pos_index = cur_return_index;
            transform.rotation = cur_move_pos_list[pos_index].rotation;
            transform.position = cur_move_pos_list[pos_index].position;

            yield return new WaitForSeconds(move_delaying);

            if(!isWaring && cur_move_pos_list[pos_index].gameObject.name == "Storage Room")//창고에서 다음위치로 이동할때까지 경보음을 안울렸으면 cctv를 끊음
                CCTV_Manger.Instance.IsBroken = true;
            pos_index++;
            isClosing = false;
            isWaring = false;
        }
    }
    IEnumerator break_CCTV()
    {
        CCTV_Manger.Instance.IsBroken = true;
        yield return new WaitForSeconds(move_delaying);
        CCTV_Manger.Instance.IsBroken = false;
    }
}
