using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class DayDoor : MonoBehaviour
{
    bool isOpen;
    Animator AN;
    SpriteRenderer SP;

    [SerializeField] Sprite OpenDoorSP;
    [SerializeField] Sprite CloseDoorSP;

    [SerializeField] GameObject Vies;


    [SerializeField] string StartDialogueID;
    private void Awake()
    {
        SP = GetComponent<SpriteRenderer>();
        //AN = GetComponent<Animator>();
    }
    public void OpenDoor()
    {
        UiUtils.GetUI<DialogueManager>().StartDialogue(StartDialogueID);
    }

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Space) && !UiUtils.GetUI<DialogueManager>().gameObject.activeSelf)
        {
            OpenDoor();
        }
        if (isOpen &&UiUtils.GetUI<DialogueManager>().gameObject.activeSelf )
        {
            Vies.gameObject.SetActive(true);
            SP.sprite = OpenDoorSP;
        }
        else
        {
            SP.sprite = CloseDoorSP;
            Vies.gameObject.SetActive(false);
        }
            

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Actor>())
        {
            isOpen = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isOpen = false;
    }
}
