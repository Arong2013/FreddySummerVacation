using UnityEditor;
using UnityEngine;

public class CCTV_Camera : Interactable_Object
{
    bool isOn = false;
    public bool OnOff {get {return isOn;} set { isOn = value;}}
    protected override void PerformInteraction()
    {
        if(!isOn)
        {
            CCTV_Manger.Instance.Get_CCTV_Select.SetActive(true);
            PlayerCamera playercam = FindObjectOfType<PlayerCamera>();//cctv를 볼때 플레이어는 움직이지않고 cctv화면만 움직임
            playercam.Lock = false;//플레이어 화면 움직임 제한
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isOn = true;
        }
    }
}
