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
    [SerializeField] private List<Dialogue> dialogues; // 대화 리스트
    private Dictionary<string, Dialogue> dialogueDictionary; // 대화 사전
    [SerializeField] private Dialogue currentDialogue;
    private bool isResponseButtonsVisible = false; // 응답 버튼의 가시성 상태
    private int currentDialogueIndex = -1; // 현재 대화 인덱스
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!isResponseButtonsVisible)
            {
                ShowNextDialogue();
            }
            else
            {
                ToggleResponseButtons();
            }
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
            dialogues = new List<Dialogue>(dialogueArray.dialogues);
            dialogueDictionary = new Dictionary<string, Dialogue>();

            foreach (var dialogue in dialogueArray.dialogues)
            {
                dialogueDictionary[dialogue.id] = dialogue;
            }
        }

        Debug.Log("끝");
    }

    public void StartDialogue(string dialogueId)
    {
        if (string.IsNullOrEmpty(dialogueId))
        {
            ShowNextDialogue();
            return;
        }

        gameObject.SetActive(true);
        if (dialogueDictionary.TryGetValue(dialogueId, out currentDialogue))
        {
            currentDialogueIndex = dialogues.IndexOf(currentDialogue);
            dialogueText.gameObject.SetActive(true);
            // 대화 텍스트를 업데이트합니다.
            ApplyTextStyles(currentDialogue.text, currentDialogue.textStyles);

            // 캐릭터 아이콘을 업데이트합니다.
          //  UpdateCharacterIcon(currentDialogue.characterIconPath);

            // 빌런 아이콘을 업데이트합니다.
         //   UpdateVillainIcon(currentDialogue.villainIconPath);

            // 응답 버튼을 업데이트합니다.
            print(currentDialogue.responses.Length);
            UpdateResponseButtons(currentDialogue.responses);
        }
    }

    private void ApplyTextStyles(string text, TextStyle[] styles)
    {
        dialogueText.text = text;
        StartCoroutine(ApplyTextStylesNextFrame(styles));
    }

    private IEnumerator ApplyTextStylesNextFrame(TextStyle[] styles)
    {
        yield return null; // 다음 프레임까지 대기

        dialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = dialogueText.textInfo;

        foreach (var style in styles)
        {
            for (int i = style.startIndex; i < style.endIndex; i++)
            {
                int meshIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
                Color32 newColor = ParseColor(style.textColor);
                vertexColors[vertexIndex + 0] = newColor;
                vertexColors[vertexIndex + 1] = newColor;
                vertexColors[vertexIndex + 2] = newColor;
                vertexColors[vertexIndex + 3] = newColor;

                // 스타일 적용
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (style.isBold)
                {
                    charInfo.style |= FontStyles.Bold;
                }
                if (style.isItalic)
                {
                    charInfo.style |= FontStyles.Italic;
                }

                // 폰트 크기 적용
                if (style.fontSize > 0)
                {
                    charInfo.scale = style.fontSize / dialogueText.fontSize;
                }

                textInfo.characterInfo[i] = charInfo;
            }
        }
        dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
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
                // 응답 버튼 텍스트를 올바르게 설정합니다.
                responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = responses[i].text;
                responseButtons[i].gameObject.SetActive(true);
                int index = i; // Capture the index for the closure
                responseButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
                responseButtons[i].onClick.AddListener(() => OnResponseSelected(index));
            }
            else
            {
               // responseButtons[i].gameObject.SetActive(false);
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
        else
        {
            ShowNextDialogue(); // 응답이 없는 경우 다음 대화로 넘어갑니다.
            HideResponseButtons(); // 응답 버튼을 숨깁니다.
        }
    }

    private void ShowNextDialogue()
    {
        if (currentDialogueIndex + 1 < dialogues.Count)
        {
            currentDialogueIndex++;
            StartDialogue(dialogues[currentDialogueIndex].id);
        }
        else
        {
            Debug.Log("No more dialogues available.");
        }
    }

    Color ParseColor(string color)
    {
        ColorUtility.TryParseHtmlString(color, out Color newCol);
        return newCol;
    }
}
