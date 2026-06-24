using TMPro;
using UnityEngine;
using System;

public class NormalButton : BaseButton
{

    public event Action OnButtonClicked;

    public bool SetButtonData(Sprite buttonSprite, string buttonText, TMP_FontAsset buttonFontAsset, Action buttonCallback)
    {
        if (ComponentCheck() == false)
        {
            return false;
        }

        if (DataCheck(buttonSprite, buttonText, buttonCallback) == false)
        {
            return false;
        }

        SetSprite(buttonSprite);
        SetButton(buttonCallback);

        if (IsText)
        {
            SetFont(buttonFontAsset);
            SetText(buttonText);
        }

        this.ActiveTrue();
        return true;
    }

    private bool ComponentCheck()
    {
        bool hasComponent = true;

        if (Button_This == null)
        {
            this.LogError("Button_This 컴포넌트가 없습니다!!");
            hasComponent = false;
        }

        if (Image_Button == null)
        {
            this.LogError("Image_Button 컴포넌트가 없습니다!!");
            hasComponent = false;
        }

        if (Text_Button == null && IsText == true)
        {
            this.LogError("Text_Button 컴포넌트가 없습니다!!");
            hasComponent = false;
        }

        if (hasComponent == false)
        {
            this.ActiveFalse();
            return false;
        }

        return true;
    }

    private bool DataCheck(Sprite buttonSprite, string buttonText, Action buttonCallback)
    {
        bool hasData = true;

        if (buttonSprite == null)
        {
            this.LogWarning("받아온 buttonSprite가 없습니다!!");
            hasData = false;
        }

        if (buttonText == null && IsText == true)
        {
            this.LogWarning("받아온 buttonText가 없습니다!!");
            hasData = false;
        }

        if (buttonCallback == null)
        {
            this.LogWarning("받아온 buttonCallback가 없습니다!!");
            hasData = false;
        }

        if (hasData == false)
        {
            this.ActiveFalse();
            return false;
        }

        return true;
    }

    private void SetSprite(Sprite buttonSprite)
    {
        Image_Button.sprite = buttonSprite;
    }

    private void SetFont(TMP_FontAsset bouttonFont)
    {
        Text_Button.font = bouttonFont;
    }

    private void SetText(string buttonText)
    {
        Text_Button.text = buttonText;
    }

    private void SetButton(Action buttonCallback)
    {
        OnButtonClicked = buttonCallback;

        BindButtonEvent();
    }

    private void BindButtonEvent()
    {
        Button_This.onClick.RemoveAllListeners();
        Button_This.onClick.AddListener(InvokeButtonClicked);
    }

    private void InvokeButtonClicked()
    {
        OnButtonClicked?.Invoke();
    }
}
