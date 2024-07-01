using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] float interactionDistance = 30.0f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] PlayerInput input;

    void Start()
    {
        input.SwitchCurrentActionMap("Control");
        input.actions["Interact"].performed += interact;
    }
    private void interact(InputAction.CallbackContext callbackContext)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, layerMask))
        {
            Interactable_Object interactableObject = hit.collider.gameObject.GetComponent<Interactable_Object>();
            interactableObject.Interact();
        }
    }
}
