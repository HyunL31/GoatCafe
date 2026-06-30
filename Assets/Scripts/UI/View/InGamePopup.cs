using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePopup : BaseUI<InGamePopup>
{
    [SerializeField] private Button Button_Background;
    [SerializeField] private Image Image_Background;

    [SerializeField] private Transform Transform_Buttons;

    public event Action OnBackgroundClicked;

    private NormalButton _tutorialButton;
    private NormalButton _gameOptionButton;
    private NormalButton _returnMainMenuButton;

    private void OnEnable()
    {
        BindButtonEvent();
    }

    private void BindButtonEvent()
    {
        Button_Background.onClick.AddListener(InvokeBackgroundClicked);
    }

    private void OnDisable()
    {
        UnBindButtonEvent();
    }

    private void UnBindButtonEvent()
    {
        Button_Background.onClick.RemoveListener(InvokeBackgroundClicked);
    }

    public void SetTutorialButton(GameObject tutorialButtonPrefab, string tutorialButtonText, Action tutorialButtonCallBack)
    {
        if(_tutorialButton != null)
        {
            return;
        }

        NormalButton tutorialButton = CreateButton(tutorialButtonPrefab, Transform_Buttons, tutorialButtonText, tutorialButtonCallBack);
        _tutorialButton = tutorialButton;
    }

    public void SetGameOptionButton(GameObject gameOptionButtonPrefab, string gameOptionButtonText, Action gaemOptionButtonCallback)
    {
        if(_gameOptionButton != null)
        {
            return;
        }

        NormalButton gameOptionButton = CreateButton(gameOptionButtonPrefab, Transform_Buttons, gameOptionButtonText, gaemOptionButtonCallback);
        _gameOptionButton = gameOptionButton;
    }

    public void SetReturnMainMenuButton(GameObject returnMainMenuButtonPrefab, string returnMainMenuButtonText, Action returnMainMenuButtonCallback)
    {
        if(_returnMainMenuButton != null)
        {
            return;
        }

        NormalButton returnMainMenuButton = CreateButton(returnMainMenuButtonPrefab, Transform_Buttons, returnMainMenuButtonText, returnMainMenuButtonCallback);
        _returnMainMenuButton = returnMainMenuButton;
    }

    private NormalButton CreateButton(GameObject buttonPrefab, Transform buttonTransform, string buttonText, Action buttonCallback)
    {
        if(this.InstantiateAndGetComponent(buttonPrefab, buttonTransform, out NormalButton normalButton) == false)
        {
            return null;
        }

        if(normalButton.SetButtonData(buttonText:buttonText,buttonCallback: buttonCallback) == false)
        {
            Destroy(normalButton.gameObject);
            return null;
        }

        return normalButton;
    }
    private void InvokeBackgroundClicked()
    {
        OnBackgroundClicked?.Invoke();
    }
}
