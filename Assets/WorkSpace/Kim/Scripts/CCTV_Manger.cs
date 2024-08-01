using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Assertions.Must;

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
public class CCTV_Manger : MonoBehaviour//싱글톤
{
    private static CCTV_Manger instance;

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
    }
    [SerializeField] List<Camera> list_cctv;
    [SerializeField] RawImage cctv_view; //cctv화면
    [SerializeField] GameObject CCTV_Select;//cctv선택화면
    [SerializeField] CCTV_Camera cctv_Monitor;//cctv모니터
    [SerializeField] PlayerCamera playercam;//플레이어 화면 움직임관련
    public GameObject Get_CCTV_Select {get { return CCTV_Select;}}
    [SerializeField] Camera cur_cam;
    public void Set_CCTV_Screen(CCTV_POS room_name)//어떤 cctv를 볼건지 선택후 실행
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CCTV_Select.SetActive(false);
        cur_cam = list_cctv[(int)room_name];//볼 CCTV바꾸고 켜기
        cur_cam.gameObject.SetActive(true);
        cctv_view.texture = cur_cam.targetTexture;
        cctv_view.gameObject.SetActive(true);
        
    }
    public void Turn_Off_CCTV(InputAction.CallbackContext callbackContext)
    {
        cur_cam.gameObject.SetActive(false);
        cctv_view.gameObject.SetActive(false);
        CCTV_Select.gameObject.SetActive(false);
        cctv_Monitor.OnOff = false;
        playercam.Lock = true;
    }
}
