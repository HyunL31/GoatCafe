using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    [SerializeField] private Button Button_This;
    [SerializeField] private Image Image_Button;
    [SerializeField] private TMP_Text Text_Button;

    [SerializeField] private bool IsText;

    private Action _buttonEvent;

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
        SetEvent(buttonCallback);

        if (IsText) SetText(buttonText, buttonFontAsset);

        this.ActiveTrue();
        return true;
    }

    private bool ComponentCheck()
    {
        bool isComponent = true;

        if (Button_This == null)
        {
            this.LogError("Button 컴포넌트가 없습니다!!");
            isComponent = false;
        }

        if (Image_Button == null)
        {
            this.LogError("Image 컴포넌트가 없습니다!!");
            isComponent = false;
        }

        if (Text_Button == null && IsText == true)
        {
            this.LogError("TMP_Text 컴포넌트가 없습니다!!");
            isComponent = false;
        }

        if (isComponent == false)
        {
            this.ActiveFalse();
            return false;
        }

        return true;
    }

    private bool DataCheck(Sprite buttonSprite, string buttonText, Action buttonCallback)
    {
        bool isData = true;

        if (buttonSprite == null)
        {
            this.LogWarning("받아온 이미지가 없습니다!!");
            isData = false;
        }

        if (buttonText == null && IsText == true)
        {
            this.LogError("받아온 텍스트가 없습니다!!");
            isData = false;
        }

        if (buttonCallback == null)
        {
            this.LogError("받아온 이벤트가 없습니다!!");
            isData = false;
        }

        if (isData == false)
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

    private void SetEvent(Action buttonCallback)
    {

        _buttonEvent = buttonCallback;
        BindButtonEvent();
    }

    private void SetText(string buttonText, TMP_FontAsset buttonFontAsset)
    {
        Text_Button.text = buttonText;
        Text_Button.font = buttonFontAsset;
    }

    private void BindButtonEvent()
    {
        Button_This.onClick.RemoveAllListeners();
        Button_This.onClick.AddListener(OnClick_Button);
    }

    private void OnClick_Button()
    {
        _buttonEvent?.Invoke();
    }
}