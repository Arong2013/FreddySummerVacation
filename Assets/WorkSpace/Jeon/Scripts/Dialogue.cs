using System;

[Serializable]
public class Dialogue
{
    public string id; 
    public string text;
    public Response[] responses;
}

[Serializable]
public class Response
{
    public string text;
    public string nextId;
}
