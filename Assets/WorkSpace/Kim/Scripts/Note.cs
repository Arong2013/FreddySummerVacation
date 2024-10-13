using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] RawImage noteCam;
    bool isOn = false;
    public bool OnOff {get {return isOn;} set { isOn = value;}}
    public void Interact()
    {
        if(!isOn)
        {
            CCTV_Manger.Instance.Get_CCTV_Select.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isOn = true;
        }
    }
    public void Turn_Off_Note(InputAction.CallbackContext callbackContext)
    {
        if(isOn)//플레이어가 멈춰있을때/게임이 정지된 상태일 때만
        {
            isOn = false;
            Cursor.visible = false;
            noteCam.gameObject.SetActive(false);
            Game_Manager.Instance.GetPlayer.IsStop = false;
        }
    }
}
