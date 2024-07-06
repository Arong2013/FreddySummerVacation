using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    bool lockRotate = true;//true일때 움직임, false일때 제한
    public float mouseSensitivity = 100.0f; // 마우스 감도
    private float xRotation = 0.0f; // x축 회전 값
    private float yRotation = 0.0f; // y축 회전 값

    public bool Lock { set {lockRotate = value;} }
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if(lockRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yRotation += mouseX; // y축 회전 값을 증가
            xRotation -= mouseY; // x축 회전 값을 감소
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // x축 회전 제한

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0.0f); // 회전을 업데이트
        }
    }
}
