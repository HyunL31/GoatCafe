using UnityEngine.Animations;

public static class AddressUtil
{
    public static class Prefab
    {
        public static class UI
        {
            public const string NormalButton = "Prefab/UI/NormalButton";

            public const string CursorUI = "Prefab/UI/CursorUI";

            public static class MainMenuUI
            {
                public const string UIPrefab = "Prefab/UI/MainMenuUI/UIPrefab";
                public const string MenuButton = "Prefab/UI/MainMenuUI/MenuButton";
            }

            public static class SaveSlotPopup
            {
                public const string PopupPrefab = "Prefab/UI/SaveSlotPopup/PopupPrefab";
                public const string NewDataButton = "Prefab/UI/SaveSlotPopup/NewDataButton";
                public const string SaveDataButton = "Prefab/UI/SaveSlotPopup/SaveDataButton";
                public const string ExitPopupButton = "Prefab/UI/SaveSlotPopup/ExitPopupButton";
            }
        }
    }

    public static class Sprite
    {
        public static class UI
        {
            public const string ButtonRed = "Sprite/UI/ButtonRed";
            public const string ButtonOrange = "Sprite/UI/ButtonOrange";
            public const string ButtonBlue = "Sprite/UI/ButtonBlue";
            public const string ButtonGreen = "Sprite/UI/ButtonGreen";

            public static class MainMenu
            {
                public const string Background = "Sprite/UI/MainMenu/Background";

                public const string Title = "Sprite/UI/MainMenu/Title";

                public const string MenuSlotEdge = "Sprite/UI/MainMenu/MenuSlotEdge";
                public const string MenuSlotBackground = "Sprite/UI/MainMenu/MenuSlotBackground";
                public const string MenuSlotTitle = "Sprite/UI/MainMenu/MenuSlotTitle";

                public const string StartButton = ButtonRed;
                public const string GameOptionButton = ButtonOrange;
                public const string ExitGameButton = ButtonBlue;
            }

            public static class SaveSlotPopup
            {
                public const string BackgroundEdge = "Sprite/UI/SaveSlotPopup/BackgroundEdge";
                public const string Background = "Sprite/UI/SaveSlotPopup/Background";
                public const string BackgroundTitle = "Sprite/UI/SaveSlotPopup/Title";

                public const string SaveSlotEdge = "Sprite/UI/SaveSlotPopup/SaveSlotEdge";
                public const string SaveSlotBackground = "Sprite/UI/SaveSlotPopup/SaveSlotBackground";
                public const string SaveSlotTitle = "Sprite/UI/SaveSlotPopup/SaveSlotTitle";

                public const string NewSlotButtonBackground = "Sprite/UI/SaveSlotPopup/NewSlotButtonBackground";

                public const string SaveButtonBlueBackground = "Sprite/UI/SaveSlotPopup/SaveButtonBlueBackground";
                public const string SaveButtonGreenBackground = "Sprite/UI/SaveSlotPopup/SaveButtonGreenBackground";
                public const string SaveButtonRedBackground = "Sprite/UI/SaveSlotPopup/SaveButtonRedBackground";

                public const string SaveButtonBlue = ButtonBlue;
                public const string SaveButtonGreen = ButtonGreen;
                public const string SaveButtonRed = ButtonRed;
                    
                public const string ExitPopupButton = "Sprite/UI/SaveSlotPopup/ExitPopupButton";
            }
        }
    }

    public static class Font
    {
        public const string BaseFont = "Font/BaseFont";
    }
}