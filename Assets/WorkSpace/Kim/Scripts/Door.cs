using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public Transform doorTransform; // 문 객체의 Transform
    public Transform doorPivotTransform; // 문 회전축 객체의 Transform
    public Transform playerCameraTransform; //플레이어 카메라의 Transform
    public Vector3 openRotation = new Vector3(0, 30, 0); // 문이 열렸을 때의 회전값
    public Vector3 closedRotation = new Vector3(0, 0, 0); // 문이 닫혔을 때의 회전값
    public float rotationSpeed = 2.0f; // 회전 속도
    public float interactionDistance = 30.0f; // 플레이어와 문의 상호작용 거리

    private bool isClosing = false; // 문이 닫히는 중인지 여부

    public Transform player; // 플레이어의 Transform
    public Vector3 lookRotation = new Vector3(45, 0, 0); // 플레이어가 고개를 숙이는 회전 값 (X축을 기준으로 45도)
    public Vector3 lookPositionOffset = new Vector3(0, -1.5f, 0.5f); // 플레이어 위치 오프셋
    public float transitionDurationToOut = 1f; // 회전 및 위치 전환 시간(밖을 보려고할때)
    public float transitionDurationToInside = 0.3f; // 회전 및 위치 전환 시간(안으로 들어올때)

    private Vector3 prevPosition;
    private Quaternion prevRotation;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    bool isLookOut = false;
    public void OpenDoor() {isClosing = false;}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = new Ray(playerCameraTransform.position, playerCameraTransform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (hit.transform == doorTransform)
                {
                    isClosing = true; // 문 닫기
                    Villain_Manager.Instance.SetTimer_Villain_E(true);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            isClosing = false; // 문 열기
            Villain_Manager.Instance.SetTimer_Villain_E(false);
        }

        // 문 회전
        if (isClosing)
        {
            doorPivotTransform.localRotation = Quaternion.Slerp(doorPivotTransform.localRotation, Quaternion.Euler(closedRotation), Time.deltaTime * rotationSpeed);
        }
        else
        {
            doorPivotTransform.localRotation = Quaternion.Slerp(doorPivotTransform.localRotation, Quaternion.Euler(openRotation), Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isClosing)
            {
                // 초기 위치와 회전 값을 저장합니다.
                prevPosition = player.position;
                prevRotation = player.rotation;

                // 목표 위치와 회전 값을 계산합니다.
                targetPosition = player.position + player.TransformDirection(lookPositionOffset);
                targetRotation = player.rotation * Quaternion.Euler(lookRotation);

                Game_Manager.Instance.GetPlayer.IsStop = true;
                isLookOut = true;
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
            player.position = Vector3.Lerp(prevPosition, targetPosition, transitionProgress);
            player.rotation = Quaternion.Slerp(prevRotation, targetRotation, transitionProgress);

            // 다음 프레임까지 대기합니다.
            yield return null;
        }

        // 전환이 완료되었으므로 목표 위치와 회전 값을 정확하게 설정합니다.
        player.position = targetPosition;
        player.rotation = targetRotation;
    }
    public void MoveBackInside(InputAction.CallbackContext callbackContext)
    {
        if(isLookOut)//플레이어가 문밖을 보고있을때만
            StartCoroutine(TransitionBackToPrevPosition());
    }

    private IEnumerator TransitionBackToPrevPosition()
    {
        float transitionProgress = 0f;

        while (transitionProgress < 1f)
        {
            // 전환 진행도를 업데이트합니다.
            transitionProgress += Time.deltaTime / transitionDurationToInside;

            // 플레이어의 위치와 회전을 부드럽게 원래 위치로 전환합니다.
            player.position = Vector3.Lerp(targetPosition, prevPosition, transitionProgress);
            player.rotation = Quaternion.Slerp(targetRotation, prevRotation, transitionProgress);

            // 다음 프레임까지 대기합니다.
            yield return null;
        }

        // 전환이 완료되었으므로 초기 위치와 회전 값을 정확하게 설정합니다.
        player.position = prevPosition;
        player.rotation = prevRotation;

        Game_Manager.Instance.GetPlayer.IsStop = false;
        isLookOut = false;
    }
}
