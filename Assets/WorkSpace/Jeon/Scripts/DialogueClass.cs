[System.Serializable]
public class Dialogue
{
    public string id;
    public string text;
    public TextStyle[] textStyles;
    public Response[] responses;
    public int characterId; // 호감도 조정을 위한 캐릭터 ID
}

[System.Serializable]
public class Response
{
    public string text;
    public string nextId;
    public int affinityChange; // 응답에 따른 호감도 변화 값
}
[System.Serializable]
public class DialogueArray
{
    public Dialogue[] dialogues;
}

[System.Serializable]
public class TextStyle
{
    public int startIndex;
    public int endIndex;
    public string textColor;
    public int fontSize;
    public bool isBold;
    public bool isItalic;
}
