
[System.Serializable]
public class Dialogue
{
    public string id; 
    public string text;
    public Response[] responses;
    public string characterIconPath; // 캐릭터 아이콘 경로 추가
    public string villainIconPath; // 빌런 아이콘 경로 추가
    public TextStyle[] textStyles; // 텍스트 스타일 추가
}

[System.Serializable]
public class Response
{
    public string text;
    public string nextId;
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
