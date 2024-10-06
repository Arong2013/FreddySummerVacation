using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain_B : Villain
{
    [SerializeField] int second_return_index;
    [SerializeField] int second_hard_return_index;
    [SerializeField] GameObject knife_on_table;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material normal;
    [SerializeField] Material getKnife;
    bool ishaving_knife = false;
    int second_cur_return_index;
    public override void Initialize(VILLAIN_DIFFICULTY difficulty = VILLAIN_DIFFICULTY.NORMAL)
    {
        meshRenderer.material = normal;
        knife_on_table.gameObject.SetActive(true);
        gameObject.SetActive(false);
        pos_index = 0;
        isAttack = false;
        isWaring = false;
        isClosing = false;
        SetDifficulty(difficulty);
    }
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
                move_delaying = 10.0f;
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
            transform.rotation = cur_move_pos_list[pos_index].rotation;
            transform.position = cur_move_pos_list[pos_index].position;

            if(cur_move_pos_list[pos_index].gameObject.name == "Lobby")//근처로 올때 발소리
            {
               Sound_Manager.Instance.PlaySFX(walking_SFX, (int)SFX_SOURCE_INDEX.DOOR_SFX);
            }

            yield return new WaitForSeconds(move_delaying);
            StartCoroutine(AttackPlayer());
            if(isWaring && cur_move_pos_list[pos_index].gameObject.name == "BreakRoom" && !ishaving_knife)//탕비실에서 다음위치로 이동할때까지 경보음을 울렸으면 식칼챙김
            {
                Debug.Log("식칼 챙김");
                meshRenderer.material = getKnife;
                knife_on_table.gameObject.SetActive(false);
                cur_return_index = second_cur_return_index;
            }
            else if(!isClosing && cur_move_pos_list[pos_index].gameObject.name == "Lobby")//로비에서 다음위치로 이동할때까지 문을 닫지않으면 플레이어 공격
            {
                Debug.Log("플레이어 공격");
                AttackPlayer();
            }
            pos_index++;
            isClosing = false;
            isWaring = false;
        }
    }
}
