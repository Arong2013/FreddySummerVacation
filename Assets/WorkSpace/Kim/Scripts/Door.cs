using UnityEngine;

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
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            isClosing = false; // 문 열기
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
}
