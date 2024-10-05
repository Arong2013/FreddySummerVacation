using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] TextMeshProUGUI time_text;
    [SerializeField] float time_text_move_duration;//시간 텍스트 이동하는데 걸리는 시간
    [SerializeField] int day = 1;
    [SerializeField] float time_delay;//1시간 지나는데 걸리는 시간 초단위
    [SerializeField] int cur_time = 0;
    [SerializeField] bool villainTest_A;
    [SerializeField] bool villainTest_B;
    [SerializeField] bool villainTest_C;
    [SerializeField] bool villainTest_D;
    [SerializeField] bool villainTest_E;
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
    public void GameStart()
    {
        player.Initialize();
        CCTV_Manger.Instance.Initialize();
        Villain_Manager.Instance.villain_Cycle(day);
        //StartCoroutine(TextMove());
        time_Coroutine = StartCoroutine(UpdateTime());
    }
    public IEnumerator GameEnd()
    {
        CCTV_Manger.Instance.GameEnd();
        Villain_Manager.Instance.GameEnd();
        Sound_Manager.Instance.StopBGM();
        StopCoroutine(time_Coroutine);
        yield return new WaitForSeconds(0);//밤씬 끝날때 연출 넣을 용도
        //낮 씬으로 넘기기
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
                //StartCoroutine(GameEnd());
            }
        }
    }
    IEnumerator TextMove()
    {   
        Vector2 targetPos = time_text.rectTransform.anchoredPosition;//텍스트가 갈 위치
        time_text.rectTransform.anchoredPosition = new Vector2(0, 0);//텍스트를 화면 중앙으로 이동
        float elapsedTime = 1;
        while (elapsedTime < time_text_move_duration)
        {
            // 경과 시간에 비례하여 현재 위치 계산
            float t = elapsedTime / time_text_move_duration;
            time_text.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0, 0), targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 목표 위치로 설정
        time_text.rectTransform.anchoredPosition = targetPos;
    }
}
