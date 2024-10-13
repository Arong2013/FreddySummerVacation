using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayDoor : MonoBehaviour
{

    SpriteRenderer SP;

    [SerializeField] Sprite OpenDoorSP;
    [SerializeField] Sprite CloseDoorSP;

    [SerializeField] GameObject Vies; // The visual or object that becomes active when door opens
    public string StartDialogueID;

    [SerializeField] AudioClip openClip, closeClip; // Separate audio clips for open and close


    DialogueManager dialogueManager;

    bool isOpenAble, isOpened;


    private void Awake()
    {
        dialogueManager = UiUtils.GetUI<DialogueManager>();
        SP = GetComponent<SpriteRenderer>();
        SP.sprite = CloseDoorSP; // 처음에는 문이 닫힌 상태로 시작
        Vies.SetActive(false);   // 문과 관련된 오브젝트가 비활성화된 상태로 시작
    }

    public void OpenDoor()
    {

        dialogueManager.StartDialogue(StartDialogueID);
        dialogueManager.dayDoor = this;
        Sound_Manager.Instance.PlaySFX(openClip);
        isOpenAble = false;
        isOpened = true;
        Vies.SetActive(true);
        SP.sprite = OpenDoorSP;
    }

    public void CloseDoor()
    {
        Sound_Manager.Instance.PlaySFX(closeClip);
        isOpenAble = false; // 문 상태를 닫힘으로 설정
        isOpened = false;
        Vies.SetActive(false);
        SP.sprite = CloseDoorSP;
    }

    private void Update()
    {

        // 문이 열려 있고, 스페이스 키가 눌렸으며, 다이얼로그가 비활성화된 경우 문을 연다
        if (isOpenAble && Input.GetKeyDown(KeyCode.Space) && !dialogueManager.gameObject.activeSelf)
        {
            OpenDoor();
            Vies.SetActive(true);
            SP.sprite = OpenDoorSP;
        }
        if (!dialogueManager.gameObject.activeSelf && isOpened)
        {
            CloseDoor();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check for collision with an Actor component to determine if door should open
        if (other.gameObject.GetComponent<Actor>())
        {
            isOpenAble = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Close the door when the object exits collision
        if (other.gameObject.GetComponent<Actor>())
        {
            isOpenAble = false;
        }
    }
}
