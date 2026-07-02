using JetBrains.Annotations;
using System;

[Serializable]
public class UIDataModel
{
    public string Id;
}

[Serializable]
public class MainMenuUIModel : UIDataModel
{
    public string MainMenuButtonPrefabPath;

    public string StartButtonText;
    public string StartButtonSpritePath;

    public string GameOptionButtonText;
    public string GameOptionButtonSpritePath;

    public string ExitGameButtonText;
    public string ExitGameButtonSpritePath;
}

[Serializable]
public class InGameUIModel : UIDataModel
{
}

[Serializable]
public class SaveDataSlotPopupModel : UIDataModel
{
    public string SaveSlotButtonPrefabPath;
    public string SaveSlotButtonParentPath;
    public string SaveSlotButtonText;
    public string SaveSlotButtonSpritePath;
}

[Serializable]
public class InGamePopupModel : UIDataModel
{
    public string TutorialButton;
    public string GameOptionButton;
    public string ReturnMainMenuButton;
}

[Serializable]
public class TutorialPopupModel : UIDataModel
{
    public string Text;
    public string PrefabPath;
}