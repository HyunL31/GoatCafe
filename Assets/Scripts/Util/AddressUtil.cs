using UnityEngine.Animations;
using UnityEngine.Rendering.UI;

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
                public const string SelectPanel = "Prefab/UI/SaveSlotPopup/SelectPanel";
                public const string NameSetPanel = "Prefab/UI/SaveSlotPopup/NameSetPanel";
                public const string ExitPopupButton = "Prefab/UI/SaveSlotPopup/ExitPopupButton";
            }

            public static class InGameUI
            {
                public const string UIPrefab = "Prefab/UI/InGameUI/UIPrefab";
                public const string DayButton = "Prefab/UI/INGameUI/DayButton";
                public const string DayChangePanel = "Prefab/UI/DayChangePanel";
            }

            public static class InGamePopup
            {
                public const string PopupPrefab = "Prefab/UI/InGamePopup/PopupPrefab";
                public const string MenuButton = "Prefab/UI/InGamePopup/MenuButton";
            }
        }
        public static class VFX
        {
            public const string SmokePoof = "Prefab/VFX/SmokePoof";
            public const string MagicPoof = "Prefab/VFX/MagicPoof";
            public const string Firework1 = "Prefab/VFX/Firework1";
            public const string Firework2 = "Prefab/VFX/Firework2";
            public const string Firework3 = "Prefab/VFX/Firework3";
            public const string HitC3D = "Prefab/VFX/HitC3D";
            public const string FlammeCrisp = "Prefab/VFX/FlameCrisp";
            public const string GasLeak = "Prefab/VFX/GasLeak";
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
            public const string ButtonRemove = "Sprite/UI/SaveSlotPopup/SlotRemoveButton";

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

                public const string SlotRemoveButton = ButtonRemove;

                public const string SaveButtonBlue = ButtonBlue;
                public const string SaveButtonGreen = ButtonGreen;
                public const string SaveButtonRed = ButtonRed;

                public const string SelectPanelBackground = "Sprite/UI/SaveSlotPopup/SelectPanelBackground";
                public const string SelectPanelYesButton = ButtonOrange;
                public const string SelectPanelNoButton = ButtonGreen;

                public const string NameSetPanelBackground = "Sprite/UI/SaveSlotPopup/NameSetPanelBackgorund";
                public const string NameSetPanelInputFieldBackground = "Sprite/UI/SaveSlotPopup/NameSetPanelInputFieldBackground";
                public const string NameSetPanelSelectButton = ButtonRed;
                public const string NameSetPanelExitButton = ExitPopupButton;

                public const string ExitPopupButton = "Sprite/UI/SaveSlotPopup/ExitPopupButton";
            }

            public static class InGameUI
            {
                public const string StaminaGaugeBackground = "Sprite/UI/InGameUI/StaminaGaugeBackground";
                public const string StaminaGauge = "Sprite/UI/InGameUI/StaminaGauge";

                public const string DayButtonBackground = "Sprite/UI/InGameUI/DayButtonBackground";
                public const string DayButtonGaugeBackground = "Sprite/UI/InGameUI/DayButtonGaugeBackground";
                public const string DayButtonGauge = "Sprite/UI/InGameUI/DayButtonGauge";

                public const string DayChangePanelBackground = "Sprite/UI/InGameUI/DayChangePanelBackground";
                public const string DayChangePanelTitle = "Sprite/UI/InGameUI/DayChangePanelTitle";
                public const string DayChangePanelEdge = "Sprite/UI/InGameUI/DayChangePanelEdge";
                public const string DayChangePanelButton = ButtonRed;
            }

            public static class InGamePopup
            {
                public const string Background = "Sprite/UI/InGamePopup/Background";
                public const string TutorialButton = "Sprite/UI/InGamePopup/TutorialButton";
                public const string GameOptionButton = "Sprite/UI/InGamePopup/GameOptionButton";
                public const string ReturnMainMenuButton = "Sprite/UI/InGamePopup/ReturnMainMenuBUtton";
            }
        }
    }

    public static class Sound
    {
        public static class BGM
        {
            public const string Bgm = "Audio/BGM/BGM_1";
        }

        public static class SFX
        {
            public const string Click1 = "Audio/SFX/UISound/Click_1";
            public const string Click2 = "Audio/SFX/UISound/Click_2";
            public const string Click3 = "Audio/SFX/UISound/Click_3";
            public const string Click4 = "Audio/SFX/UISound/Click_4";

            public const string Hover1 = "Audio/SFX/UISound/Hover_1";
            public const string Hover2 = "Audio/SFX/UISound/Hover_2";
        }
    }

    public static class Font
    {
        public const string BaseFont = "Font/BaseFont";
    }
}