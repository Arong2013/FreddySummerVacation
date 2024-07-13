using System;

[Serializable]
public class Dialogue
{
    public string id; 
    public string text;
    public Response[] responses;
    public string characterIconPath; // 캐릭터 아이콘 경로 추가
}
[Serializable]
public class Response
{
    public string text;
    public string nextId;
}
