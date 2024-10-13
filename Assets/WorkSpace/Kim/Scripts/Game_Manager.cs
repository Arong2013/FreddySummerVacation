using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Game_Manager : Singleton<Game_Manager>
{
    [SerializeField] Player player;
    [SerializeField] PlayerInput input;
    [SerializeField] SceneChange sceneChanger;
    [SerializeField] Door door;
    [SerializeField] Note note;
    [SerializeField] TextMeshProUGUI time_text;
    [SerializeField] float time_text_move_duration; //시간 텍스트 이동하는데 걸리는 시간
    [SerializeField] int day = 1;
    [SerializeField] float time_delay; //1시간 지나는데 걸리는 시간 초단위
    [SerializeField] int cur_time = 0;
    [SerializeField] bool villainTest_A = false;
    [SerializeField] bool villainTest_B = false;
    [SerializeField] bool villainTest_C = false;
    [SerializeField] bool villainTest_D = false;
    [SerializeField] bool villainTest_E = false;
    Coroutine time_Coroutine;
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

        Villain_Manager.Instance.Initialize_All_Villains();
        if(villainTest_A)
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.A);//빌런 테스트용
        if(villainTest_B)
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.B);
        if(villainTest_C)
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.C);
        if(villainTest_D)
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.D);
        if(villainTest_E)
            Villain_Manager.Instance.StartMove(VILLAIN_INDEX.E);
        GameStart();
    }
    public void SetInputAction(bool status)
    {
        if(status)
        {
            input.SwitchCurrentActionMap("Control");
            input.actions["Interact"].started += player.interact;
            input.actions["ESC"].started += CCTV_Manger.Instance.Turn_Off_CCTV;
            input.actions["ESC"].started += door.MoveBackInside;
            input.actions["ESC"].started += note.Turn_Off_Note;
        }
        else
        {
            input.SwitchCurrentActionMap("Control");
            input.actions["Interact"].started -= player.interact;
            input.actions["ESC"].started -= CCTV_Manger.Instance.Turn_Off_CCTV;
            input.actions["ESC"].started -= door.MoveBackInside;
            input.actions["ESC"].started -= note.Turn_Off_Note;
        }
    }
    public void GameStart()
    {
        SetInputAction(true);
        player.Initialize();
        CCTV_Manger.Instance.Initialize();
        Villain_Manager.Instance.villain_Cycle(day);
        sceneChanger.GameStart();
        time_Coroutine = StartCoroutine(UpdateTime());
    }
    public void GameEnd(bool clearStatus)
    {
        StopCoroutine(time_Coroutine);
        CCTV_Manger.Instance.GameEnd();
        Villain_Manager.Instance.GameEnd();
        Sound_Manager.Instance.StopBGM();
        player.GameEnd();
        SetInputAction(false);
        Cursor.visible = false;
        if(clearStatus)
            StartCoroutine(sceneChanger.ChangeScene("DayScene"));
        else
            StartCoroutine(sceneChanger.ChangeScene("TitleScene"));
    }
    IEnumerator UpdateTime()
    {
        while(true)
        {
            time_text.text = $"{cur_time} : 00";//Interpolated Strings
            yield return new WaitForSeconds(time_delay);
            cur_time++;
            if(cur_time >= 6)//6시가 되면 밤씬 끝
            {
                time_text.text = $"{cur_time} : 00";
                day++;
                GameEnd(true);
            }
        }
    }
}
