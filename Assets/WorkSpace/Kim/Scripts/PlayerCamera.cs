using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    bool lockRotate = false;//false일때 움직임, true일때 제한
    public float mouseSensitivity; // 마우스 감도
    private float xRotation = 0.0f; // x축 회전 값
    private float yRotation = -90.0f; // y축 회전 값

    public bool Lock { set {lockRotate = value;} }
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if(!lockRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yRotation += mouseX; // y축 회전 값을 증가
            xRotation -= mouseY; // x축 회전 값을 감소
            xRotation = Mathf.Clamp(xRotation, -10f, 50f); // x축 회전 제한
            yRotation = Mathf.Clamp(yRotation, -200f, 20f); // x축 회전 제한

            transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f); // 회전을 업데이트
            mainCamera.transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, 0);
        }
    }
}
