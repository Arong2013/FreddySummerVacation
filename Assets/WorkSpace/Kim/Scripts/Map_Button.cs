using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Button : MonoBehaviour
{
    [SerializeField] CCTV_POS pos;
    
    public void OnClick()
    {
        CCTV_Manger.Instance.Set_CCTV_Screen(pos);
    }
}
