using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText; // 대화 텍스트 UI 요소
    [SerializeField] private Button[] responseButtons; // 응답 버튼 UI 요소
    [SerializeField] private Image characterIcon, NpcIcon, VillainIcon; // 캐릭터 아이콘 UI 요소 추가
    [SerializeField] private string googleSheetUrl; // Google Sheets에서 제공한 웹 앱 URL
    private Dictionary<string, Dialogue> dialogues;
    [SerializeField] private Dialogue currentDialogue;
    private bool isResponseButtonsVisible = false; // 응답 버튼의 가시성 상태

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleResponseButtons();
        }
    }

    public IEnumerator LoadDialoguesFromGoogleSheet()
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
        }

        Debug.Log("끝");
    }

    public void StartDialogue(string dialogueId)
    {
        gameObject.SetActive(true);
        if (dialogues.TryGetValue(dialogueId, out currentDialogue))
        {
            dialogueText.gameObject.SetActive(true);
            // 대화 텍스트를 업데이트합니다.
            dialogueText.text = currentDialogue.text;

            // 캐릭터 아이콘을 업데이트합니다.
            UpdateCharacterIcon(currentDialogue.characterIconPath);

            // 빌런 아이콘을 업데이트합니다.
            UpdateVillainIcon(currentDialogue.villainIconPath);

            // 응답 버튼을 업데이트합니다.
            UpdateResponseButtons(currentDialogue.responses);
        }
    }

    void UpdateCharacterIcon(string iconPath)
    {
        // 캐릭터 아이콘을 로드하여 UI에 설정합니다.
        Sprite newIcon = Resources.Load<Sprite>(iconPath);
        if (newIcon != null)
        {
            NpcIcon.sprite = newIcon;
        }
        else
        {
            Debug.LogError("Failed to load character icon: " + iconPath);
        }
    }

    void UpdateVillainIcon(string iconPath)
    {
        // 빌런 아이콘을 로드하여 UI에 설정합니다.
        Sprite newIcon = Resources.Load<Sprite>(iconPath);
        if (newIcon != null)
        {
            VillainIcon.sprite = newIcon;
        }
        else
        {
            Debug.LogError("Failed to load villain icon: " + iconPath);
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

    void ToggleResponseButtons()
    {
        isResponseButtonsVisible = !isResponseButtonsVisible;
        dialogueText.gameObject.SetActive(false);
        foreach (var button in responseButtons)
        {
            button.gameObject.SetActive(isResponseButtonsVisible);
        }
    }

    void HideResponseButtons()
    {
        isResponseButtonsVisible = false;
        foreach (var button in responseButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void OnResponseSelected(int responseIndex)
    {
        if (responseIndex < currentDialogue.responses.Length)
        {
            string nextId = currentDialogue.responses[responseIndex].nextId;
            StartDialogue(nextId);
        }
        HideResponseButtons(); // 응답 버튼을 숨깁니다.
    }
}
