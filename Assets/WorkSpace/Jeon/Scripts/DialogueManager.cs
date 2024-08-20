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

    private void OnDisable() => Time.timeScale = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
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
                // 시퀀스 ID가 같은 경우에만 다음 대화로 이동
                if (currentDialogueIndex + 1 < dialogues.Count)
                {
                    var nextDialogue = dialogues[currentDialogueIndex + 1];
                    if (currentDialogue.sequence == nextDialogue.sequence)
                    {
                        currentDialogueIndex++;
                        StartDialogue(nextDialogue.id);
                    }
                    else
                    {
                        print("끝");
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    gameObject.SetActive(false);
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
        gameObject.SetActive(true);

        if (string.IsNullOrEmpty(dialogueId))
        {
            // 아이디가 없을 경우 대화 텍스트만 표시
            if (currentDialogue != null)
            {
                dialogueText.gameObject.SetActive(true);
                StartCoroutine(TypeText(currentDialogue.text, currentDialogue.textStyles));
            }
            else
            {
                Debug.LogError("No dialogue available to display.");
            }
            return;
        }

        if (dialogueDictionary.TryGetValue(dialogueId, out currentDialogue))
        {
            currentDialogueIndex = dialogues.IndexOf(currentDialogue);
            dialogueText.gameObject.SetActive(true);
            UpdateIcons(currentDialogue.characterIconPath, currentDialogue.villainIconPath);

            // ID에 따라 텍스트 정렬 설정
            if (dialogueId == "P")
            {
                dialogueText.alignment = TextAlignmentOptions.Left;
            }
            else if (dialogueId == "M")
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
            gameObject.SetActive(false);
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

        // 응답이 있을 경우에만 버튼을 표시
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
            // 응답이 없을 경우 모든 버튼을 숨김
            foreach (var button in responseButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
    public void OnResponseSelected(int responseIndex)
    {
        var response = currentDialogue.responses[responseIndex];
        if (!string.IsNullOrEmpty(response.nextId))
        {
            // 다음 대화의 시퀀스 ID와 상관없이 대화를 진행
            if (dialogueDictionary.TryGetValue(response.nextId, out var nextDialogue))
            {
                StartDialogue(nextDialogue.id);
            }
            else
            {
                gameObject.SetActive(false); // 다음 대화를 찾지 못한 경우 대화 종료
            }
        }
        else
        {
            gameObject.SetActive(false); // 다음 ID가 없으면 대화 종료
        }
    }

    private void ShowNextDialogue()
    {
        if (currentDialogueIndex + 1 < dialogues.Count)
        {
            var nextDialogue = dialogues[currentDialogueIndex + 1];

            // 다음 대화의 시퀀스 ID가 다를 경우
            if (currentDialogue.sequence != nextDialogue.sequence)
            {
                gameObject.SetActive(false);
                return;
            }

            currentDialogueIndex++;
            StartDialogue(nextDialogue.id);
        }
        else
        {
            gameObject.SetActive(false);
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
}
