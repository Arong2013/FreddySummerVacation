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
    [SerializeField] private Image CharacterIcon,VillainIcon;
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
                if (currentDialogue.responses.Length < 1)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    ToggleResponseButtons(true);
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
                dialogueDictionary[dialogue.id] = dialogue;
        }
        print("끝");
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
            UpdateIcons(currentDialogue.characterIconPath, currentDialogue.villainIconPath);
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
        foreach (var button in responseButtons)
        {
            button.gameObject.SetActive(false);
        }

        if (show)
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
            }
        }
    }

    public void OnResponseSelected(int responseIndex)
    {
        foreach (var button in responseButtons)
        {
            button.gameObject.SetActive(false);
        }

        var response = currentDialogue.responses[responseIndex];
        if (!string.IsNullOrEmpty(response.nextId))
        {
            StartDialogue(response.nextId);
        }
        else
        {
            gameObject.SetActive(false);
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
