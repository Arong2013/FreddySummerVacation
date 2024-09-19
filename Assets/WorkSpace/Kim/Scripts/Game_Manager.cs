using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Game_Manager : Singleton<Game_Manager>
{
/*     private static Game_Manager instance;

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
    } */
    [SerializeField] Player player;
    [SerializeField] PlayerInput input;
    [SerializeField] Door door;
    [SerializeField] Note note;
    [SerializeField] bool isTesting;
    public Player GetPlayer => player;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        input.SwitchCurrentActionMap("Control");
        input.actions["Interact"].started += player.interact;
        input.actions["ESC"].started += CCTV_Manger.Instance.Turn_Off_CCTV;
        input.actions["ESC"].started += door.MoveBackInside;
        input.actions["ESC"].started += note.Turn_Off_Note;
        //input.actions["Move"].performed += player.Move;//플레이어 움직임
        //input.actions["Move"].canceled += player.Move;
        if(isTesting)
        {
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.A, VILLAIN_DIFFICULTY.NORMAL);//빌런 테스트용
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.B, VILLAIN_DIFFICULTY.NORMAL);
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.C, VILLAIN_DIFFICULTY.NORMAL);
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.D, VILLAIN_DIFFICULTY.NORMAL);
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.E, VILLAIN_DIFFICULTY.NORMAL);
        }

        player.Initialize();
        CCTV_Manger.Instance.Initialize();
    }
}
