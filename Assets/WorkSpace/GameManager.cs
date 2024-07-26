using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        StartCoroutine(UiUtils.GetUI<DialogueManager>().LoadDialoguesFromGoogleSheet());
        
    }
}
