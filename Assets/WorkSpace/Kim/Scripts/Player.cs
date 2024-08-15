using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] float interactionDistance = 30.0f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] float moveSpeed = 5f; // 플레이어 이동 속도
    Vector3 velocity = Vector3.zero;
    Vector3 dir = Vector3.zero;
    bool isStop = false;//true일때 멈춤
    public bool IsStop { get { return isStop;} set {isStop = value; playerCamera.Lock = value;} }
    public void Initialize()
    {
        isStop = false;
    }
    public void interact(InputAction.CallbackContext callbackContext)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, layerMask))
        {
            if(hit.collider.gameObject.tag == "CCTV MONITOR")
            {
                CCTV_Camera cctv_cam = hit.collider.gameObject.GetComponent<CCTV_Camera>();
                cctv_cam.Interact();
                playerCamera.Lock = true;//플레이어 화면 움직임 제한
                isStop = true;
            }
        }
    }
    void Update()
    {
        if(!IsStop)
            transform.Translate(velocity * Time.deltaTime);
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();
        dir = new Vector3(v.x, 0, v.y);
        velocity = dir * moveSpeed;
    }
}
