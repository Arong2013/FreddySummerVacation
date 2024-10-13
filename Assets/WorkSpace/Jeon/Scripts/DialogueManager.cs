using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button[] responseButtons;
    [SerializeField] private Image CharacterIcon, VillainIcon;
    [SerializeField] private string googleSheetUrl;
    [SerializeField] private List<Dialogue> dialogues;
    private Dictionary<string, Dialogue> dialogueDictionary;
    private Dialogue currentDialogue;
    private int currentDialogueIndex = -1;
    private bool isTyping = false;
    private bool isTextDisplayed = false;
    private bool canProceed = true; // K 키 입력을 막기 위한 변수
    public DayDoor dayDoor;

    [SerializeField] AudioClip audioClip;
    [SerializeField] private float spaceCooldown = 1f; // 스페이스 입력 제한 시간 (1초)
    private float lastSpaceTime = 0f; // 마지막 스페이스 입력 시간

    private void OnDisable()
    {
        Time.timeScale = 1f;
        dayDoor.StartDialogueID = currentDialogue.id;
    }

    void Update()
    {
        // 리스폰이 있는 경우에는 입력을 막음
        if (Input.GetKeyDown(KeyCode.Space) && canProceed)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentDialogue.text;
                ApplyTextStyles(currentDialogue.textStyles);
                isTyping = false;
                isTextDisplayed = true;
            }
            else if (isTextDisplayed)
            {
                if (currentDialogueIndex + 1 < dialogues.Count)
                {               
                    if (currentDialogue.responses != null && currentDialogue.responses.Length > 0)
                    {
                        ToggleResponseButtons(true); // 응답 버튼 활성화
                        canProceed = false; // 응답 버튼이 있는 경우 스페이스 입력을 막음
                        return;
                    }
                    else
                    {
                        ToggleResponseButtons(false); // 응답 버튼이 없는 경우 비활성화
                    }

                    var nextDialogue = dialogues[currentDialogueIndex + 1];               // 대화 시퀀스가 다를 경우 대화를 종료
                    if (currentDialogue.sequence != nextDialogue.sequence)
                    {
                        EndDialogue();  // 대화 종료 처리
                        return;
                    }

                    currentDialogueIndex++;
                    StartDialogue(nextDialogue.id);
                }
                else
                {
                    // 더 이상 대화가 없을 경우 대화 종료
                    ToggleResponseButtons(false); // 버튼 비활성화
                    EndDialogue();  // 대화 종료 처리
                }
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
            DialogueArray dialogueArray = JsonUtility.FromJson<DialogueArray>(www.downloadHandler.text);
            dialogues = new List<Dialogue>(dialogueArray.dialogues);
            dialogueDictionary = new Dictionary<string, Dialogue>();
            foreach (var dialogue in dialogues)
            {
                dialogueDictionary[dialogue.id] = dialogue;
            }
        }
        Debug.Log("끝");
    }

    public void StartDialogue(string dialogueId)
    {
        if (dialogueDictionary.TryGetValue(dialogueId, out currentDialogue))
        {
            gameObject.SetActive(true);
            currentDialogueIndex = dialogues.IndexOf(currentDialogue);
            dialogueText.gameObject.SetActive(true);
            UpdateIcons(currentDialogue.characterIconPath, currentDialogue.villainIconPath);

            if (currentDialogue.characterId == "P")
            {
                dialogueText.alignment = TextAlignmentOptions.Left;
            }
            else if (currentDialogue.characterId == "N")
            {
                dialogueText.alignment = TextAlignmentOptions.Center;
            }
            else
            {
                dialogueText.alignment = TextAlignmentOptions.Right;
            }

            StartCoroutine(TypeText(currentDialogue.text, currentDialogue.textStyles));
        }
        else
        {
            Debug.LogError("Dialogue ID not found: " + dialogueId);
            EndDialogue();  // 대화 종료 처리
        }
    }

    private IEnumerator TypeText(string text, TextStyle[] styles)
    {
        dialogueText.text = "";
        isTyping = true;
        isTextDisplayed = false;
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        ApplyTextStyles(styles);
        isTyping = false;
        isTextDisplayed = true;
    }

    private void ApplyTextStyles(TextStyle[] styles)
    {
        dialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = dialogueText.textInfo;
        foreach (var style in styles)
        {
            for (int i = style.startIndex; i < style.endIndex; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                Color32 newColor = ParseColor(style.textColor);
                for (int j = 0; j < 4; j++)
                    vertices[charInfo.vertexIndex + j] = newColor;

                if (style.isBold) charInfo.style |= FontStyles.Bold;
                if (style.isItalic) charInfo.style |= FontStyles.Italic;
                if (style.fontSize > 0) charInfo.scale = style.fontSize / dialogueText.fontSize;

                textInfo.characterInfo[i] = charInfo;
            }
        }
        dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
    private void ToggleResponseButtons(bool show)
    {
        dialogueText.gameObject.SetActive(!show);

        if (show && currentDialogue.responses.Length > 0)
        {
            for (int i = 0; i < responseButtons.Length; i++)
            {
                if (i < currentDialogue.responses.Length)
                {
                    responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.responses[i].text;
                    responseButtons[i].gameObject.SetActive(true);
                    int index = i;
                    responseButtons[i].onClick.RemoveAllListeners();
                    responseButtons[i].onClick.AddListener(() => OnResponseSelected(index));
                }
                else
                {
                    responseButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (var button in responseButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
    public void OnResponseSelected(int responseIndex)
    {
        Sound_Manager.Instance.PlaySFX(audioClip);
        canProceed = true; // 리스폰을 선택한 후에는 K 키 입력을 다시 허용
        dialogueText.gameObject.SetActive(true); // 대화 텍스트 다시 활성화
        ToggleResponseButtons(false); // 리스폰 버튼 비활성화

        var response = currentDialogue.responses[responseIndex];

        // 호감도 변화를 적용
        if (response.affinityChange != 0 && !string.IsNullOrEmpty(currentDialogue.characterId))
        {
            GameManager.Instance.UpdateAffinity(currentDialogue.characterId, response.affinityChange);
        }

        if (!string.IsNullOrEmpty(response.nextId))
        {
            if (dialogueDictionary.TryGetValue(response.nextId, out var nextDialogue))
            {
                StartDialogue(nextDialogue.id);
            }
            else
            {
                EndDialogue();  // 대화 종료 처리
            }
        }
        else
        {
            EndDialogue();  // 대화 종료 처리
        }
    }


    private void ShowNextDialogue()
    {
        if (currentDialogueIndex + 1 < dialogues.Count)
        {
            var nextDialogue = dialogues[currentDialogueIndex + 1];

            // 시퀀스가 다르면 게임 오브젝트를 비활성화하고 위치를 (0, 0, 0)으로 변경
            if (currentDialogue.sequence != nextDialogue.sequence)
            {
                EndDialogue();  // 대화 종료 처리
                return;
            }

            currentDialogueIndex++;
            StartDialogue(nextDialogue.id);
        }
        else
        {
            EndDialogue();  // 대화 종료 처리
        }
    }

    private void UpdateIcons(string characterIconPath, string villainIconPath)
    {
        if (!string.IsNullOrEmpty(characterIconPath))
        {
            Sprite newCharacterIcon = Resources.Load<Sprite>(characterIconPath);
            if (newCharacterIcon != null)
            {
                CharacterIcon.sprite = newCharacterIcon;
            }
            else
            {
                Debug.LogError("Failed to load character icon: " + characterIconPath);
            }
        }

        if (!string.IsNullOrEmpty(villainIconPath))
        {
            Sprite newVillainIcon = Resources.Load<Sprite>(villainIconPath);
            if (newVillainIcon != null)
            {
                VillainIcon.sprite = newVillainIcon;
            }
            else
            {
                Debug.LogError("Failed to load villain icon: " + villainIconPath);
            }
        }
    }

    private Color32 ParseColor(string color) => ColorUtility.TryParseHtmlString(color, out Color newCol) ? newCol : Color.white;

    private void EndDialogue()
    {
        gameObject.SetActive(false);  // 대화 창을 비활성화
        //transform.position = Vector3.zero;  // 위치를 (0, 0, 0)으로 설정
        // 필요한 추가적인 종료 작업이 있다면 여기에 작성
    }
}
