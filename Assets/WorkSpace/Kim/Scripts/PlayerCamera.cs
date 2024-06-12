using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 100.0f; // 마우스 감도

    private float xRotation = 0.0f; // x축 회전 값
    private float yRotation = 0.0f; // y축 회전 값

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 화면 중앙에 고정
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX; // y축 회전 값을 증가
        xRotation -= mouseY; // x축 회전 값을 감소
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // x축 회전 제한

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0.0f); // 회전을 업데이트
    }
}
