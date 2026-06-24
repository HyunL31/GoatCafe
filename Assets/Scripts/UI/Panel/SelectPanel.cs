using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour
{
    [SerializeField] private Image Image_Background;

    [Serializable]
    private class Buttons
    {
        public Button Button_This;
        public Image Image_Button;
        public TMP_Text Text_Button;
    }

    [SerializeField] private Buttons YesButton;
    [SerializeField] private Buttons NoButton;

    [SerializeField] private TMP_Text Text_Title;
    [SerializeField] private TMP_Text Text_Description;

    private Action OnYesButtonClicked;
    private Action OnNoButtonClicked;

    public void Open()
    {
        this.ActiveTrue();
    }

    public void Close()
    {
        this.ActiveFalse();
    }

    public void SetSelectPanel(Sprite backgroundSprite, Sprite yesButtonSprite, Sprite noButtonSprite, TMP_FontAsset panelFontAsset)
    {
        Image_Background.sprite = backgroundSprite;

        YesButton.Image_Button.sprite = yesButtonSprite;
        YesButton.Text_Button.font = panelFontAsset;

        NoButton.Image_Button.sprite = noButtonSprite;
        NoButton.Text_Button.font = panelFontAsset;

        Text_Title.font = panelFontAsset;
        Text_Description.font = panelFontAsset;
    }

    public void BindYesButtonEvent(Action yesButtonCallback)
    {
        OnYesButtonClicked = yesButtonCallback;

        YesButton.Button_This.onClick.RemoveAllListeners();
        YesButton.Button_This.onClick.AddListener(InvokeYesButtonClicked);
    }

    public void BindNoButtonEvent(Action noButtonCallback)
    {
        OnNoButtonClicked = noButtonCallback;

        NoButton.Button_This.onClick.RemoveAllListeners();
        NoButton.Button_This.onClick.AddListener(InvokeNoButtonClicked);
    }

    public void SetText(string titleText, string descriptionText, string yesButtonText, string noButtonText)
    {
        Text_Title.text = titleText;

        Text_Description.text = descriptionText;

        YesButton.Text_Button.text = yesButtonText;

        NoButton.Text_Button.text = noButtonText;
    }

    private void InvokeYesButtonClicked()
    {
        OnYesButtonClicked?.Invoke();
        Close();
    }

    private void InvokeNoButtonClicked()
    {
        OnNoButtonClicked?.Invoke();
        Close();
    }
}
