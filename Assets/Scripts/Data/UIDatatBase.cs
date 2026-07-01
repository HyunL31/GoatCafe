using System;

[Serializable]
public class UIModelBase
{
    public string Id;
}

[Serializable]
public class MainMenuUIModel : UIModelBase
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
public class InGamePopupData : UIModelBase
{
    public string TutorialButton;
    public string GameOptionButton;
    public string ReturnMainMenuButton;
}

[Serializable]
public class TutorialPopupData : UIModelBase
{
    public string Text;
    public string PrefabPath;
}

[Serializable]
public class TutorialPopupTextData : UIModelBase
{
    public string Title;
    public string Description;
}