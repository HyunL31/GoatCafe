using System;

[Serializable]
public class UIDataBase
{
    public string Id;
}

[Serializable]
public class MainMenuUIData : UIDataBase
{
    public string MainMenuButtonPath;

    public string StartButtonText;
    public string StartButtonSpritePath;

    public string GameOptionButtonText;
    public string GameOptionButtonSpritePath;

    public string ExitGameButtonText;
    public string ExitGameButtonSpritePath;
}

[Serializable]
public class InGamePopupData : UIDataBase
{
    public string TutorialButton;
    public string GameOptionButton;
    public string ReturnMainMenuButton;
}

[Serializable]
public class TutorialPopupData : UIDataBase
{
    public string Text;
    public string PrefabPath;
}

[Serializable]
public class TutorialPopupTextData : UIDataBase
{
    public string Title;
    public string Description;
}