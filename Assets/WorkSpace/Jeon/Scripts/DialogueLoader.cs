using UnityEngine;
using System.Collections.Generic;

public static class DialogueLoader
{
    public static Dictionary<string, Dialogue> LoadDialogues(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        DialogueArray dialoguesArray = JsonUtility.FromJson<DialogueArray>(jsonFile.text);
        Dictionary<string, Dialogue> dialogues = new Dictionary<string, Dialogue>();

        foreach (var dialogue in dialoguesArray.dialogues)
        {
            dialogues[dialogue.id] = dialogue;
        }

        return dialogues;
    }
}