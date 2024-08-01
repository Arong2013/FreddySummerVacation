using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV_Move : MonoBehaviour
{
    /*자동으로 움직임
    public float rotationSpeed = 10f;
    private float rotationY;
    */
    bool lockRotate = true;//직접 움직임
    [SerializeField] float mouseSensitivity = 100.0f; // 마우스 감도
    private float xRotation = 0.0f; // x축 회전 값
    private float yRotation = 0.0f; // y축 회전 값
    public bool Lock { set {lockRotate = value;} }
    [SerializeField] Camera cctvCamera;    // CCTV 카메라
    [SerializeField] Villain enemy;     // 적 게임 오브젝트
    private bool isEnemyDetected = false;
    Coroutine coroutine;

    void CheckEnemyInView()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cctvCamera);//cctv의 시야를 구하는 함수
        if (GeometryUtility.TestPlanesAABB(planes, enemy.GetComponent<Collider>().bounds))//cctv의 시야안에 대상이 있는지 확인하는 함수
        {
            isEnemyDetected = true;
        }
        else
        {
            isEnemyDetected = false;
        }
    }

    void EndGame()
    {
        // 게임 종료 처리
        Debug.Log("Enemy detected for 5 seconds. Game Over!");
        // 예: 게임 오버 화면 표시, 게임 멈추기 등
    }
    void Update()
    {
        /*rotationY += rotationSpeed * Time.deltaTime; //자동
        transform.localRotation = Quaternion.Euler(0, rotationY, 0);*/

        if(lockRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yRotation += mouseX; // y축 회전 값을 증가
            xRotation -= mouseY; // x축 회전 값을 감소
            xRotation = Mathf.Clamp(xRotation, 0f, 60f); // x축 회전 제한
            yRotation = Mathf.Clamp(yRotation, 0f, 70f); // y축 회전 제한

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0.0f); // 회전을 업데이트
        }

        CheckEnemyInView();

        if (isEnemyDetected)
        {
            coroutine = StartCoroutine(enemy.CheckTime());
        }
        else
        {
            StopCoroutine(coroutine);
        }
    }
}
