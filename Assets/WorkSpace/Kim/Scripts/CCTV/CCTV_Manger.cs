using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Assertions.Must;
using UnityEngine.Analytics;

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
/*     private static CCTV_Manger instance;

    // CCTV_Manger 인스턴스에 접근할 수 있는 프로퍼티
    public static CCTV_Manger Instance
    {
        get
        {
            if (instance == null)
            {
                // Scene에서 CCTV_Manger 찾아서 인스턴스화한다.
                instance = FindObjectOfType<CCTV_Manger>();

                // Scene에 CCTV_Manger 없으면 새로 생성한다.
                if (instance == null)
                {
                    GameObject obj = new GameObject("CCTV_Manager");
                    instance = obj.AddComponent<CCTV_Manger>();
                }
            }
            return instance;
        }
    } */
    [SerializeField] List<Camera> list_cctv;
    [SerializeField] RawImage cctv_view; //cctv화면
    [SerializeField] Texture broken_cctv_view; //끊긴 cctv화면
    [SerializeField] Image noise_image; //cctv선택창에 노이즈이미지
    [SerializeField] GameObject CCTV_Select;//cctv선택화면
    [SerializeField] CCTV_Camera cctv_Monitor;//cctv모니터
    [SerializeField] PlayerCamera playercam;//플레이어 화면 움직임관련
    public GameObject Get_CCTV_Select {get { return CCTV_Select;}}
    public bool IsOn_CCTV { set{ isOn_CCTV = value; } get { return isOn_CCTV; }}
    [SerializeField] Camera cur_cam;
    bool isBroken = false;
    bool isOn_CCTV = false;
    public bool IsBroken { set{ isBroken = value; noise_image.gameObject.SetActive(value); } get { return isBroken; } }
    public void Initialize()
    {
        isBroken = false;
        isOn_CCTV = false;
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
        
    }
    public void Turn_Off_CCTV(InputAction.CallbackContext callbackContext)
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
        }
    }
}
