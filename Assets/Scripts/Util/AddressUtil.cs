public static class AddressUtil
{
    public static class Prefab
    {
        public static class UI
        {
            public const string BaseButton = "Prefab/UI/BaseButton";
            public const string MainMenuUI = "Prefab/UI/MainMenuUI";
        }
    }

    public static class Sprite
    {
        public static class UI
        {
            public const string ButtonRed = "Sprite/UI/ButtonRed";
            public const string ButtonOrange = "Sprite/UI/ButtonOrange";
            public const string ButtonBlue = "Sprite/UI/ButtonBlue";

            public static class MainMenu
            {
                public const string Background = "Sprite/UI/MainMenu/Background";
                public const string StartButton = ButtonRed;
                public const string GameOptionButton = ButtonOrange;
                public const string ExitGameButton = ButtonBlue;
            }

            public static class SaveSlotPopup
            {
                public const string Background = "Sprite/UI/SaveSlotPopup/Background";
                public const string TitleImage = "Sprite/UI/SaveSlotPopup/TitleImage";
                public const string SaveButton = "Sprite/UI/SaveSlotPopup/SaveButton";
            }
        }
    }

    public static class Font
    {
        public const string BaseFont = "Font/BaseFont";
    }
}