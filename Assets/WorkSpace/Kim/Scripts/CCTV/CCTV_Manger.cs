using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Assertions.Must;
using UnityEngine.Analytics;
using UnityEngine.Audio;
using TMPro;
using UnityEditor.ShaderKeywordFilter;

public enum CCTV_POS
{
    R202, R206, R207,
    STAIRS_2F, STAIRS_1F,//계단
    CORRIDOR,//복도
    LAUNDRY_ROOM,//세탁실
    STORAGE_ROOM,//창고
    LOBBY,//로비
    BREAK_ROOM,//탕비실
}
public class CCTV_Manger : Singleton<CCTV_Manger>
{
    [SerializeField] List<Camera> list_cctv;
    [SerializeField] RawImage cctv_view; //cctv화면
    [SerializeField] Texture broken_cctv_view; //끊긴 cctv화면
    [SerializeField] Image noise_image; //cctv선택창에 노이즈이미지
    [SerializeField] GameObject CCTV_Select;//cctv선택화면
    [SerializeField] CCTV_Camera cctv_Monitor;//cctv모니터
    [SerializeField] PlayerCamera playercam;//플레이어 화면 움직임관련
    [SerializeField] AudioClip closeCCTV_Clip;
    [SerializeField] AudioClip cctv_bgm;//cctv를 볼 동안 나올 bgm
    [SerializeField] AudioClip origin_bgm;//원래 bgm
    [SerializeField] TextMeshProUGUI cctv_Battery_Text;
    public GameObject Get_CCTV_Select {get { return CCTV_Select;}}
    public bool IsOn_CCTV { set{ isOn_CCTV = value; } get { return isOn_CCTV; }}
    [SerializeField] Camera cur_cam;
    [SerializeField] float max_cctv_battery; 
    [SerializeField] float cctv_battery_watching_decrease_delay;//cctv를 보고 있을때 줄어드는 속도
    [SerializeField] float cctv_battery_decrease_delay;//cctv배터리가 줄어드는 속도
    [SerializeField] bool is_cctv_battery_down;//cctv배터리가 깍이고 있는지
    public bool Is_cctv_battery_down { set{ is_cctv_battery_down = value; } get { return is_cctv_battery_down; }}
    bool isBroken = false;
    bool isOn_CCTV = false;
    float cur_cctv_battery;
    float cur_cctv_battery_decrease_delay;
    Coroutine cctv_battery_down_Coroutine;
    public bool IsBroken { set{ isBroken = value; noise_image.gameObject.SetActive(value); } get { return isBroken; } }
    public void Initialize()
    {
        isBroken = false;
        isOn_CCTV = false;
        cur_cctv_battery = max_cctv_battery;
        cctv_battery_down_Coroutine = StartCoroutine(CCTV_Battery_Down());
    }
    public void GameEnd()
    {
        StopCoroutine(cctv_battery_down_Coroutine);
        Turn_Off_CCTV();
    }
    public void Set_CCTV_Screen(CCTV_POS room_name)//어떤 cctv를 볼건지 선택후 실행
    {
        isOn_CCTV = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Game_Manager.Instance.GetPlayer.IsStop = true;
        playercam.Lock = true;
        CCTV_Select.SetActive(false);
        if(isBroken)
        {
            cctv_view.texture = broken_cctv_view;
            cctv_view.gameObject.SetActive(true);
        }
        else
        {
            cur_cam = list_cctv[(int)room_name];//볼 CCTV바꾸고 켜기
            cur_cam.gameObject.SetActive(true);
            cctv_view.texture = cur_cam.targetTexture;
            cctv_view.gameObject.SetActive(true);
        }
        Sound_Manager.Instance.PlayBGM(cctv_bgm);
        DecreaseBattery();
        cur_cctv_battery_decrease_delay = cctv_battery_watching_decrease_delay;
    }
    public void Turn_Off_CCTV(InputAction.CallbackContext callbackContext)
    {
        Turn_Off_CCTV();
    }
    public void Turn_Off_CCTV()
    {
        if(Game_Manager.Instance.GetPlayer.IsStop && isOn_CCTV)//플레이어가 멈춰있을때/게임이 정지된 상태일 때만
        {
            isOn_CCTV = false;
            Cursor.visible = false;
            cur_cam.gameObject.SetActive(false);
            cctv_view.gameObject.SetActive(false);
            CCTV_Select.gameObject.SetActive(false);
            cctv_Monitor.OnOff = false;
            playercam.Lock = false;
            Game_Manager.Instance.GetPlayer.IsStop = false;
            Sound_Manager.Instance.PlaySFX(closeCCTV_Clip, (int)SFX_SOURCE_INDEX.NORMAL_SFX);
            Sound_Manager.Instance.PlayBGM(origin_bgm);
            cur_cctv_battery_decrease_delay = cctv_battery_decrease_delay;
        }
    }
    public IEnumerator CCTV_Battery_Down()
    {
        while(is_cctv_battery_down)
        {
            yield return new WaitForSeconds(cctv_battery_decrease_delay);
            DecreaseBattery();
            if(cur_cctv_battery <= 0)
            {
                CCTV_Down();
            }
        }
    }
    void CCTV_Down()
    {
        //cctv배터리 다 됬을때
    }
    public void DecreaseBattery(float decreaseAmount = 1)
    {
        cur_cctv_battery -= decreaseAmount;
        if(cur_cctv_battery <= 0)
            cctv_Battery_Text.text = "0";
        else
            cctv_Battery_Text.text = $"{cur_cctv_battery}%";//Interpolated Strings
    }
}
