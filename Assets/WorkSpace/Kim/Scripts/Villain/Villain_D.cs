using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Villain_D : Villain
{
    [SerializeField] AudioClip neighbor_noise;
    bool isNoising = false;//층간소음 내는 중인지
    public override IEnumerator Move()
    {
        while(!isAttack)
        {
            transform.rotation = cur_move_pos_list[pos_index].rotation;
            transform.position = cur_move_pos_list[pos_index].position;

            if(cur_move_pos_list[pos_index].gameObject.name == "Lobby")//근처로 올때 발소리
            {
               Sound_Manager.Instance.PlaySFX(walking_SFX, (int)SFX_SOURCE_INDEX.DOOR_SFX);
            }
            else if(cur_move_pos_list[pos_index].gameObject.name == "None")
            {
               //사라져서 층간소음내기 시작하고 다음위치로 갔을때 소음멈추기
               isNoising = true;
               StartCoroutine(Play_Neighbor_Noise());
            }
            yield return new WaitForSeconds(move_delaying);
            //문을 닫았었는지 확인
            Debug.Log(cur_move_pos_list[pos_index].gameObject.name);
            if(!isClosing && cur_move_pos_list[pos_index].gameObject.name == "Lobby")//로비에서 다음위치로 이동할때까지 문을 닫지않으면 플레이어 공격
            {
                Debug.Log("플레이어 공격");
                StartCoroutine(AttackPlayer());
            }
            if(pos_index + 1 >= cur_move_pos_list.Length) pos_index = cur_return_index;
            else pos_index++;
            isClosing = false;
            isWaring = false;
            isNoising = false;
        }
    }
    IEnumerator Play_Neighbor_Noise()
    {
        while(isNoising)
        {
            if(!Sound_Manager.Instance.IsPlayingAudioSource(neighbor_noise, (int)SFX_SOURCE_INDEX.NEIGHBOR_NOISE))
                Sound_Manager.Instance.PlaySFX(neighbor_noise, (int)SFX_SOURCE_INDEX.NEIGHBOR_NOISE);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
