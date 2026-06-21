using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SaveDataButton : BaseButton
{
    [SerializeField] private Image Image_Background;

    [SerializeField] private TMP_Text Text_DataName;

    [SerializeField] private TMP_Text Text_DayTitle;
    [SerializeField] private TMP_Text Text_GoldTitle;
    [SerializeField] private TMP_Text Text_StaminaTitle;

    [SerializeField] private TMP_Text Text_DayData;
    [SerializeField] private TMP_Text Text_GoldData;
    [SerializeField] private TMP_Text Text_StaminaData;

    private Action<int> OnButtonClicked;
    private int _index;

    public bool SetButtonData(Sprite backgroundSprite, Sprite buttonSprite, string dayTitle, string goldTitle, string staminaTitle, TMP_FontAsset buttonFont, int index, PlayerModel playerModel, Action<int> ButtonClickedCallback)
    {
        if (ComponentCheck() == false)
        {
            return false;
        }

        if (DataCheck(backgroundSprite, buttonSprite, dayTitle, goldTitle, staminaTitle, buttonFont, playerModel, ButtonClickedCallback) == false)
        {
            return false;
        }

        SetSprite(backgroundSprite, buttonSprite);
        SetFont(buttonFont);
        SetText(dayTitle, goldTitle, staminaTitle);

        SetSaveData(index, playerModel);
        SetButton(index, ButtonClickedCallback);

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

        if(Image_Background == null)
        {
            this.LogError("Image_Background 컴포넌트가 없습니다!!");
            hasComponent = false;
        }

        if (Image_Button == null)
        {
            this.LogError("Image_Button 컴포넌트가 없습니다!!");
            hasComponent = false;
        }

        if (hasComponent == false)
        {
            this.ActiveFalse();
            return false;
        }

        return true;
    }

    private bool DataCheck(Sprite backgroundSprite, Sprite buttonSprite, string dayTitle, string goldTitle, string staminaTitle, TMP_FontAsset buttonFont, PlayerModel playerModel, Action<int> ButtonClickedCallback)
    {
        bool hasData = true;

        if (backgroundSprite == null)
        {
            this.LogWarning("받아온 backgroundSprite가 없습니다!!");
            hasData = false;
        }

        if (buttonSprite == null)
        {
            this.LogWarning("받아온 buttonSprite가 없습니다!!");
            hasData = false;
        }

        if (dayTitle == null)
        {
            this.LogWarning("받아온 dayTitle가 없습니다!!");
            hasData = false;
        }

        if(goldTitle == null)
        {
            this.LogWarning("받아온 goldTitle가 없습니다!!");
            hasData = false;
        }

        if(staminaTitle == null)
        {
            this.LogWarning("받아온 staminaTitle가 없습니다!!");
            hasData = false;
        }

        if(buttonFont == null)
        {
            this.LogError("받아온 폰트가 없습니다!!");
            hasData = false;
        }

        if (playerModel == null)
        {
            this.LogError("받아온 세이브 데이터가 없습니다");
            hasData = false;
        }

        if(ButtonClickedCallback == null)
        {
            this.LogWarning("받아온 이벤트가 없습니다!!");
            hasData = false;
        }

        if (hasData == false)
        {
            this.ActiveFalse();
            return false;
        }

        return true;

    }

    private void SetSprite(Sprite backgroundSprite, Sprite buttonSprite)
    {
        Image_Background.sprite = backgroundSprite;
        Image_Button.sprite = buttonSprite;
    }

    private void SetFont(TMP_FontAsset buttonFont)
    {
        Text_Button.font = buttonFont;

        Text_DataName.font = buttonFont;

        Text_DayTitle.font = buttonFont;
        Text_GoldTitle.font = buttonFont;
        Text_StaminaTitle.font = buttonFont;

        Text_DayData.font = buttonFont;
        Text_GoldData.font = buttonFont;
        Text_StaminaData.font = buttonFont;
    }

    private void SetText(string dayTitle, string goldTitle, string staminaTitle)
    {
        Text_DayTitle.text = dayTitle;
        Text_GoldTitle.text = goldTitle;
        Text_StaminaTitle.text = staminaTitle;
    }

    public void SetSaveData(int index, PlayerModel playerModel)
    {
        Text_DataName.text = index.ToString();
        Text_DayData.text = playerModel.Day.ToString();
        Text_GoldData.text = playerModel.Gold.ToString();
        Text_StaminaData.text = playerModel.Stamina.ToString();
    }

    private void SetButton(int index, Action<int> buttonCallback)
    {
        OnButtonClicked = buttonCallback;
        _index = index;

        BindButtonEvent();
    }

    private void BindButtonEvent()
    {
        Button_This.onClick.RemoveAllListeners();
        Button_This.onClick.AddListener(InvokeButtonClicked);
    }

    private void InvokeButtonClicked()
    {
        OnButtonClicked?.Invoke(_index);
    }
}
