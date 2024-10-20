using UnityEditor;
using UnityEngine;

public class CCTV_Camera : MonoBehaviour//CCTV 모니터가 가질 스크립트
{
    [SerializeField] AudioClip click_Clip;
    bool isOn = false;
    public bool OnOff {get {return isOn;} set { isOn = value;}}
    public void Interact()
    {
        if(!isOn && !Game_Manager.Instance.IsGameStop)
        {
            CCTV_Manger.Instance.Get_CCTV_Select.SetActive(true);
            CCTV_Manger.Instance.IsOn_CCTV = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Sound_Manager.Instance.PlaySFX(click_Clip, (int)SFX_SOURCE_INDEX.NORMAL_SFX);
            isOn = true;
        }
    }
}
