using System;

[Serializable]
public class GameDataBase
{
    public string ID;
}

[Serializable]
public class DialogueData : GameDataBase
{
    public string NextID;
    public string Speaker;
    public string Content;
    public string Background;
    public string BGM;
}

