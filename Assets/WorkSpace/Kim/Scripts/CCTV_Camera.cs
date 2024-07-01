using UnityEngine;

public class CCTV_Camera : Interactable_Object
{
    bool OnOff = false;
    [SerializeField] GameObject CCTV_Display;
    protected override void PerformInteraction()
    {
        OnOff = !OnOff;
        CCTV_Display.gameObject.SetActive(OnOff);
        Debug.Log(OnOff);
        PlayerCamera playercam = FindObjectOfType<PlayerCamera>();//cctv를 볼때 플레이어는 움직이지않고 cctv화면만 움직임
        playercam.Lock = !OnOff;
        CCTV_Move cctv_Move = FindObjectOfType<CCTV_Move>();
        cctv_Move.Lock = OnOff;
    }
}
