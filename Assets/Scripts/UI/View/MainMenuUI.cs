using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : BaseUI<MainMenuUI>
{
    [SerializeField] private Image Image_Background;

    [SerializeField] private Image Image_Title;

    [SerializeField] private Image Image_MenuSlotEdge;
    [SerializeField] private Image Image_MenuSlotBackground;
    [SerializeField] private Image Image_MenuSlotTitle;

    [SerializeField] private Transform Transform_StartButton;
    [SerializeField] private Transform Transform_GameOptionButton;
    [SerializeField] private Transform Transform_ExitGameButton;

    public void SetBackgroundImage(Sprite backgroundSprite)
    {
        Image_Background.sprite = backgroundSprite;
    }

    public void SetTitleImage(Sprite titleSprite)
    {
        Image_Title.sprite = titleSprite;
    }

    public void SetMenuSlotImage(Sprite menuSlotEdgeSprite, Sprite menuSlotBackgroundSprite, Sprite menuSlotTitleSprite = null)
    {
        Image_MenuSlotEdge.sprite = menuSlotEdgeSprite;
        Image_MenuSlotBackground.sprite = menuSlotBackgroundSprite;

        if (menuSlotTitleSprite == null)
        {
            Image_MenuSlotTitle.ActiveFalse();
        }
        else
        {
            Image_MenuSlotTitle.sprite = menuSlotTitleSprite;
            Image_MenuSlotTitle.ActiveTrue();
        }
    }
    public void SetStartButton(GameObject startButtonPrefab, Sprite startButtonSprite, string startButtonText, TMP_FontAsset startButtonFont, Action startButtonCallback)
    {
        CreateButton(startButtonPrefab, Transform_StartButton, startButtonSprite, startButtonText, startButtonFont, startButtonCallback);
    }
    public void SetGameOptionButton(GameObject GameOptionButtonPrefab, Sprite GameOptionButtonSprite, string gameOptionButtonText, TMP_FontAsset gameOptionButtonFont, Action gaemOptionButtonCallback)
    {
        CreateButton(GameOptionButtonPrefab, Transform_GameOptionButton, GameOptionButtonSprite, gameOptionButtonText, gameOptionButtonFont, gaemOptionButtonCallback);
    }

    public void SetExitGameButton(GameObject exitGameButtonPrefab, Sprite exitGameButtonSprite, string exitGameButtonText, TMP_FontAsset exitGameButtonFont, Action exitGameButtonCallBack)
    {
        CreateButton(exitGameButtonPrefab, Transform_ExitGameButton, exitGameButtonSprite, exitGameButtonText, exitGameButtonFont, exitGameButtonCallBack);
    }

    private void CreateButton(GameObject buttonPrefab, Transform buttonTransform, Sprite buttonSprite, string buttonText, TMP_FontAsset buttonFont, Action buttonCallback)
    {
        if (this.InstantiateAndGetComponent(buttonPrefab, buttonTransform, out NormalButton normalButton) == false)
        {
            return;
        }

        if (normalButton.SetButtonData(buttonSprite, buttonText, buttonFont, buttonCallback) == false)
        {
            Destroy(normalButton.gameObject);
        }
    }
}
