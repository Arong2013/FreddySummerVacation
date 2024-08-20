using System;
using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string id;                // 대화의 고유 ID
    public string sequence;          // 시퀀스 ID (문자열로 변경)
    public string text;              // 대화 텍스트
    public TextStyle[] textStyles;   // 텍스트 스타일 정보
    public Response[] responses;     // 대화에 대한 응답
    public string characterId;       // 호감도 조정을 위한 캐릭터 ID
    public string characterIconPath; // 캐릭터 아이콘 경로
    public string villainIconPath;   // 빌런 아이콘 경로
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
