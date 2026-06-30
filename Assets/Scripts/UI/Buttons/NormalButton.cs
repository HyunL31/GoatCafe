using TMPro;
using UnityEngine;
using System;

public class NormalButton : BaseButton
{
    public event Action OnButtonClicked;

    private void OnEnable()
    {
        BindButtonEvent();
    }

    private void BindButtonEvent()
    {
        Button_This.onClick.AddListener(InvokeButtonClicked);
    }

    private void OnDisable()
    {
        UnBindButtonEvent();
    }

    private void UnBindButtonEvent()
    {
        Button_This.onClick.RemoveListener(InvokeButtonClicked);
    }

    private void OnDestroy()
    {
        ResetButtonEvent();
    }

    public bool SetButtonData(Sprite buttonSprite = null, string buttonText = null)
    {
        if (buttonSprite != null)
        {
            SetSprite(buttonSprite);
        }

        if ((IsText == true) && (buttonText != null))
        {
            SetText(buttonText);
        }

        this.ActiveTrue();
        return true;
    }

    private void SetSprite(Sprite buttonSprite)
    {
        Image_Button.sprite = buttonSprite;
    }

    private void SetText(string buttonText)
    {
        Text_Button.text = buttonText;
    }

    private void InvokeButtonClicked()
    {
        OnButtonClicked?.Invoke();
    }

    public void ResetButtonEvent()
    {
        OnButtonClicked = null;
    }
}
