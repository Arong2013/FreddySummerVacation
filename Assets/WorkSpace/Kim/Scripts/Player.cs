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
    bool isStop = false;
    public bool IsStop { get { return isStop;} set {isStop = value; playerCamera.Lock = value;} }
    public void interact(InputAction.CallbackContext callbackContext)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, layerMask))
        {
            Interactable_Object interactableObject = hit.collider.gameObject.GetComponent<Interactable_Object>();
            interactableObject.Interact();
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
