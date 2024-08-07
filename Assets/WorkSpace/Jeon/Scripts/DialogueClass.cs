using System;
using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string id;
    public string text;
    public TextStyle[] textStyles;
    public Response[] responses;
    public string characterId; // 호감도 조정을 위한 캐릭터 ID
    public string characterIconPath; // 캐릭터 아이콘 경로
    public string villainIconPath; // 빌런 아이콘 경로
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
