using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Game_Manager : MonoBehaviour
{
    private static Game_Manager instance;

    // Game_Manager 인스턴스에 접근할 수 있는 프로퍼티
    public static Game_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                // Scene에서 Game_Manager 찾아서 인스턴스화한다.
                instance = FindObjectOfType<Game_Manager>();

                // Scene에 Game_Manager 없으면 새로 생성한다.
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<Game_Manager>();
                }
            }
            return instance;
        }
    }
    [SerializeField] Player player;
    [SerializeField] PlayerInput input;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        input.SwitchCurrentActionMap("Control");
        input.actions["Interact"].performed += player.interact;
        input.actions["ESC"].performed += CCTV_Manger.Instance.Turn_Off_CCTV;
    }
}
