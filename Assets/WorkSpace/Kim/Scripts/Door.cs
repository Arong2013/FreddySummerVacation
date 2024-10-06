using System.Collections;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    [SerializeField] Transform doorTransform; // 문 객체의 Transform
    [SerializeField] Transform doorPivotTransform; // 문 회전축 객체의 Transform
    [SerializeField] Transform playerCameraTransform; //플레이어 카메라의 Transform
    [SerializeField] Vector3 openRotation = new Vector3(0, 45, 0); // 문이 열렸을 때의 회전값
    [SerializeField] Vector3 closedRotation = new Vector3(0, 0, 0); // 문이 닫혔을 때의 회전값
    [SerializeField] Vector3 forced_openRotation = new Vector3(0, 120, 0); // 강제로 문이 열렸을 때의 회전값
    [SerializeField] float rotationSpeed = 2.0f; // 회전 속도
    [SerializeField] float interactionDistance = 30.0f; // 플레이어와 문의 상호작용 거리
    [SerializeField] Transform playerOriginPos; //원래 플레이어의 위치
    [SerializeField] AudioClip closeDoor_Clip;
    [SerializeField] AudioClip openDoor_Clip;
    private bool isClosing = false; // 문이 닫히는 중인지 여부

    [SerializeField] Transform player; // 플레이어의 Transform
    //public Vector3 lookRotation = new Vector3(45, 0, 0); // 플레이어가 고개를 숙이는 회전 값 (X축을 기준으로 45도)
    //public Vector3 lookPositionOffset = new Vector3(0, -1.5f, 0.5f); // 플레이어 위치 오프셋
    [SerializeField] Transform lookOutPos; //문밖을 쳐다볼때 움직일 위치
    [SerializeField] float transitionDurationToOut; // 회전 및 위치 전환 시간(밖을 보려고할때)
    [SerializeField] float transitionDurationToInside; // 회전 및 위치 전환 시간(안으로 들어올때)

    private Vector3 prevPosition;
    private Quaternion prevRotation;
    //private Vector3 targetPosition;
    //private Quaternion targetRotation;
    bool isLookOut = false; //밖을 보고있을때
    bool forced_Open_door = false;
    bool isDoorFullyClosed = false;
    void Update()
    {
        if(forced_Open_door)//강제로 문이 열리면 다른 행동 불가
        {
            doorPivotTransform.localRotation = Quaternion.Slerp(doorPivotTransform.localRotation, Quaternion.Euler(forced_openRotation), Time.deltaTime * rotationSpeed);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F) && isLookOut) //밖을 보고있을때 문을 닫을수있음
            {
                CloseDoor();
            }
            else if (Input.GetKeyUp(KeyCode.F) && isLookOut && isClosing)
            {
                OpenDoor();
            }

            // 문 회전
            if (!isClosing)//문 열림
            {
                doorPivotTransform.localRotation = Quaternion.Slerp(doorPivotTransform.localRotation, Quaternion.Euler(openRotation), Time.deltaTime * rotationSpeed);
            }
        }
    }
    public void CloseDoor()
    {
        isClosing = true;
        MoveBackInside();
        StartCoroutine(CloseDoorCoroutine());
        Villain_Manager.Instance.SetTimer_Villain_E(true);
        Villain_Manager.Instance.Door_Closing();
    }
    public void OpenDoor()
    {
        isClosing = false; // 문 열기
        Game_Manager.Instance.GetPlayer.IsStop = false;
        isLookOut = false;
        Villain_Manager.Instance.SetTimer_Villain_E(false);
        Villain_Manager.Instance.Door_Open();
        if(isDoorFullyClosed)//문이 완전히 닫혔다가 열릴때만 소리나게
            Sound_Manager.Instance.PlaySFX(openDoor_Clip, (int)SFX_SOURCE_INDEX.DOOR_SFX);
        isDoorFullyClosed = false;
    }
    public void Forced_OpenDoor()
    {
        if(isLookOut)
            MoveBackInside();
        isClosing = false; // 문 열기
        Game_Manager.Instance.GetPlayer.IsStop = false;
        isLookOut = false;
        forced_Open_door = true;
        Sound_Manager.Instance.PlaySFX(openDoor_Clip, (int)SFX_SOURCE_INDEX.DOOR_SFX);
    }
/*     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isClosing)
            {
                // 초기 위치와 회전 값을 저장합니다.
                prevPosition = player.position;
                prevRotation = player.rotation;

                // 목표 위치와 회전 값을 계산합니다.
                //targetPosition = player.position + player.TransformDirection(lookPositionOffset);
                //targetRotation = player.rotation * Quaternion.Euler(lookRotation);

                Game_Manager.Instance.GetPlayer.IsStop = true;
                isLookOut = true;
                // 전환 시작
                StartCoroutine(TransitionToLookOutside());
            }
            
        }
    } */
    public void LookOut()
    {
        {
            if(!isClosing)
            {
                // 초기 위치와 회전 값을 저장합니다.
                prevPosition = player.position;
                prevRotation = player.rotation;

                // 목표 위치와 회전 값을 계산합니다.
                //targetPosition = player.position + player.TransformDirection(lookPositionOffset);
                //targetRotation = player.rotation * Quaternion.Euler(lookRotation);

                Game_Manager.Instance.GetPlayer.IsStop = true;
                // 전환 시작
                StartCoroutine(TransitionToLookOutside());
            }
        }
    }

    private IEnumerator TransitionToLookOutside()
    {
        float transitionProgress = 0f;

        while (transitionProgress < 1f)
        {
            // 전환 진행도를 업데이트합니다.
            transitionProgress += Time.deltaTime / transitionDurationToOut;

            // 플레이어의 위치와 회전을 부드럽게 전환합니다.
            player.position = Vector3.Lerp(prevPosition, lookOutPos.position, transitionProgress);
            player.rotation = Quaternion.Slerp(prevRotation, lookOutPos.rotation, transitionProgress);

            // 다음 프레임까지 대기합니다.
            yield return null;
        }
        // 전환이 완료되었으므로 목표 위치와 회전 값을 정확하게 설정합니다.
        player.position = lookOutPos.position;
        player.rotation = lookOutPos.rotation;

        isLookOut = true;
    }
    public void MoveBackInside()//문을 닫으면서 뒤로 이동
    {
        if(isLookOut)//플레이어가 문밖을 보고있을때만
            StartCoroutine(TransitionBackToPrevPosition());
    }
    public void MoveBackInside(InputAction.CallbackContext callbackContext)//문이 열린채로 뒤로 이동
    {
        if(isLookOut && !isClosing)//플레이어가 문밖을 보고있고 문을 닫지않았을때만
        {
            StartCoroutine(TransitionBackToPrevPosition());
            Game_Manager.Instance.GetPlayer.IsStop = false;
            isLookOut = false;
            Villain_Manager.Instance.SetTimer_Villain_E(false);
        }
    }


    public IEnumerator TransitionBackToPrevPosition()
    {
        float transitionProgress = 0f;

        while (transitionProgress < 1f)
        {
            // 전환 진행도를 업데이트합니다.
            transitionProgress += Time.deltaTime / transitionDurationToInside;

            // 플레이어의 위치와 회전을 부드럽게 원래 위치로 전환합니다.
            player.position = Vector3.Lerp(lookOutPos.position, prevPosition, transitionProgress);
            player.rotation = Quaternion.Slerp(lookOutPos.rotation, prevRotation, transitionProgress);

            // 다음 프레임까지 대기합니다.
            yield return null;
        }
        // 전환이 완료되었으므로 초기 위치와 회전 값을 정확하게 설정합니다.
        player.position = prevPosition;
        player.rotation = prevRotation;
    }

    IEnumerator CloseDoorCoroutine()
    {
        while(isClosing)
        {
            if(doorPivotTransform.localRotation == Quaternion.Euler(closedRotation) && !isDoorFullyClosed)
            {
                Sound_Manager.Instance.PlaySFX(closeDoor_Clip, (int)SFX_SOURCE_INDEX.DOOR_SFX);
                isDoorFullyClosed = true;
                isLookOut = false;
            }
            else
            {
                doorPivotTransform.localRotation = Quaternion.Slerp(doorPivotTransform.localRotation, Quaternion.Euler(closedRotation), Time.deltaTime * rotationSpeed);
            }
            yield return null;
        }
    }
}
