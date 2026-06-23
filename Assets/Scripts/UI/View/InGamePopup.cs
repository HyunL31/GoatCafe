using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePopup : BaseUI<InGamePopup>
{
    [SerializeField] private Button Button_Background;
    [SerializeField] private Image Image_Background;

    [SerializeField] private Transform Transform_Buttons;

    private Action OnBackgroundClicked;

    private NormalButton _tutorialButton;
    private NormalButton _gameOptionButton;
    private NormalButton _returnMainMenuButton;

    public void SetBackgroundImageAndAction(Sprite backgroundSprite, Action backgroundCallback)
    {
        Image_Background.sprite = backgroundSprite;

        SetButton(backgroundCallback);
    }

    public void SetTutorialButton(GameObject tutorialButtonPrefab, Sprite tutorialButtonSprite, string tutorialButtonText, TMP_FontAsset tutorialButtonFont, Action tutorialButtonCallBack)
    {
        if(_tutorialButton != null)
        {
            return;
        }

        NormalButton tutorialButton = CreateButton(tutorialButtonPrefab, Transform_Buttons, tutorialButtonSprite, tutorialButtonText, tutorialButtonFont, tutorialButtonCallBack);
        _tutorialButton = tutorialButton;
    }

    public void SetGameOptionButton(GameObject gameOptionButtonPrefab, Sprite gameOptionButtonSprite, string gameOptionButtonText, TMP_FontAsset gameOptionButtonFont, Action gaemOptionButtonCallback)
    {
        if(_gameOptionButton != null)
        {
            return;
        }

        NormalButton gameOptionButton = CreateButton(gameOptionButtonPrefab, Transform_Buttons, gameOptionButtonSprite, gameOptionButtonText, gameOptionButtonFont, gaemOptionButtonCallback);
        _gameOptionButton = gameOptionButton;
    }

    public void SetReturnMainMenuButton(GameObject returnMainMenuButtonPrefab, Sprite returnMainMenuButtonSprite, string returnMainMenuButtonText, TMP_FontAsset returnMainMenuButtonFont, Action returnMainMenuButtonCallback)
    {
        if(_returnMainMenuButton != null)
        {
            return;
        }

        NormalButton returnMainMenuButton = CreateButton(returnMainMenuButtonPrefab, Transform_Buttons, returnMainMenuButtonSprite, returnMainMenuButtonText, returnMainMenuButtonFont, returnMainMenuButtonCallback);
        _returnMainMenuButton = returnMainMenuButton;
    }

    private NormalButton CreateButton(GameObject buttonPrefab, Transform buttonTransform, Sprite buttonSprite, string buttonText, TMP_FontAsset buttonFont, Action buttonCallback)
    {
        if(this.InstantiateAndGetComponent(buttonPrefab, buttonTransform, out NormalButton normalButton) == false)
        {
            return null;
        }

        if(normalButton.SetButtonData(buttonSprite, buttonText, buttonFont, buttonCallback) == false)
        {
            Destroy(normalButton.gameObject);
            return null;
        }

        return normalButton;
    }

    private void SetButton(Action backgroundCallback)
    {
        OnBackgroundClicked = backgroundCallback;

        BindBackgroundEvent();
    }

    private void BindBackgroundEvent()
    {
        Button_Background.onClick.RemoveAllListeners();
        Button_Background.onClick.AddListener(InvokeBackgroundClicked);
    }

    private void InvokeBackgroundClicked()
    {
        OnBackgroundClicked?.Invoke();
    }
}
