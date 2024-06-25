using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText; // 대화 텍스트 UI 요소
    [SerializeField] private Button[] responseButtons; // 응답 버튼 UI 요소
    [SerializeField] private string googleSheetUrl; // Google Sheets에서 제공한 웹 앱 URL
    private Dictionary<string, Dialogue> dialogues;
    private Dialogue currentDialogue;

    void Start()
    {
        StartCoroutine(LoadDialoguesFromGoogleSheet());
    }
    IEnumerator LoadDialoguesFromGoogleSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(googleSheetUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load dialogues: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            DialogueArray dialogueArray = JsonUtility.FromJson<DialogueArray>(json);
            dialogues = new Dictionary<string, Dialogue>();

            foreach (var dialogue in dialogueArray.dialogues)
            {
                dialogues[dialogue.id] = dialogue;
            }

            // 초기 대화 시작 (예: "greeting")
            StartDialogue("greeting");
        }
    }


    public void StartDialogue(string dialogueId)
    {
        if (dialogues.TryGetValue(dialogueId, out currentDialogue))
        {
            // 대화 텍스트를 업데이트합니다.
            dialogueText.text = currentDialogue.text;

            // 응답 버튼을 업데이트합니다.
            UpdateResponseButtons(currentDialogue.responses);
        }
    }

    void UpdateResponseButtons(Response[] responses)
    {
        for (int i = 0; i < responseButtons.Length; i++)
        {
            if (i < responses.Length)
            {
                responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = responses[i].text;
                responseButtons[i].gameObject.SetActive(true);
                int index = i; // Capture the index for the closure
                responseButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
                responseButtons[i].onClick.AddListener(() => OnResponseSelected(index));
            }
            else
            {
                responseButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnResponseSelected(int responseIndex)
    {
        if (responseIndex < currentDialogue.responses.Length)
        {
            string nextId = currentDialogue.responses[responseIndex].nextId;
            StartDialogue(nextId);
        }
    }
}