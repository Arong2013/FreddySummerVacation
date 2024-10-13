using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Button : MonoBehaviour
{
    [SerializeField] CCTV_POS pos;
    [SerializeField] AudioClip Click_Clip = null;
    
    public void OnClick()//CCTV
    {
        CCTV_Manger.Instance.Set_CCTV_Screen(pos);
        Sound_Manager.Instance.PlaySFX(Click_Clip, (int)SFX_SOURCE_INDEX.NORMAL_SFX);
    }
    public void OnClickWarning()
    {
        if(!CCTV_Manger.Instance.IsBroken)
        {
            Villain_Manager.Instance.Warning();
            Sound_Manager.Instance.PlaySFX(Click_Clip, (int)SFX_SOURCE_INDEX.NORMAL_SFX);
        }
    }
}
