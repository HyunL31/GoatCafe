using System;

[Serializable]
public class UIDataBase
{
    public string Id;
}

[Serializable]
public class MainMenuUIData : UIDataBase
{
    public string StartButton;
    public string GameOptionButton;
    public string ExitGameButton;
}