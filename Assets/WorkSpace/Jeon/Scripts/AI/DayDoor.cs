using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayDoor : MonoBehaviour
{
    Animator AN;
    private void Awake()
    {
        AN = GetComponent<Animator>();
    }
    public void OpenDoor()
    {
        AN.SetTrigger("Open");
        UiUtils.GetUI<DialogueManager>().StartDialogue("Day1_P_B1_0");
    }
}