using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataButton : BaseButton
{
    [SerializeField] private Button Button_Remove;

    [SerializeField] private Image Image_Background;
    [SerializeField] private Image Image_RemoveButton;

    [SerializeField] private TMP_Text Text_DataName;

    [SerializeField] private TMP_Text Text_DayTitle;
    [SerializeField] private TMP_Text Text_CoinTitle;
    [SerializeField] private TMP_Text Text_StaminaTitle;

    [SerializeField] private TMP_Text Text_DayData;
    [SerializeField] private TMP_Text Text_CoinData;
    [SerializeField] private TMP_Text Text_StaminaData;

    private Action<string> OnButtonClicked;
    private Action<string, SaveDataButton> OnRemoveButtonClicked;
    private string _index;

    public bool SetButtonData(Sprite backgroundSprite, Sprite buttonSprite, Sprite removeSprite, string dayTitle, string CoinTitle, string staminaTitle, string startButton, TMP_FontAsset buttonFont, string slotname, PlayerModel playerModel, Action<string> ButtonClickedCallback, Action<string, SaveDataButton> RemoveButtonClickedCallback)
    {
        if (ComponentCheck() == false)
        {
            return false;
        }

        if (DataCheck(backgroundSprite, buttonSprite, dayTitle, CoinTitle, staminaTitle, startButton, buttonFont, playerModel, ButtonClickedCallback, RemoveButtonClickedCallback) == false)
        {
            return false;
        }

        SetSprite(backgroundSprite, buttonSprite, removeSprite);
        SetFont(buttonFont);
        SetText(dayTitle, CoinTitle, staminaTitle, startButton, playerModel);

        SetSaveData(slotname, playerModel);
        SetButton(slotname, ButtonClickedCallback);
        SetRemoveButton(RemoveButtonClickedCallback);

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

        if(Image_RemoveButton == null)
        {
            this.LogError("Image_RemoveButton 컴포넌트가 없습니다!!");
            hasComponent = false;
        }

        if (hasComponent == false)
        {
            this.ActiveFalse();
            return false;
        }

        return true;
    }

    private bool DataCheck(Sprite backgroundSprite, Sprite buttonSprite, string dayTitle, string CoinTitle, string staminaTitle, string startButton, TMP_FontAsset buttonFont, PlayerModel playerModel, Action<string> ButtonClickedCallback, Action<string, SaveDataButton> RemoveButtonClickedCallback)
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

        if(CoinTitle == null)
        {
            this.LogWarning("받아온 CoinTitle가 없습니다!!");
            hasData = false;
        }

        if(staminaTitle == null)
        {
            this.LogWarning("받아온 staminaTitle가 없습니다!!");
            hasData = false;
        }

        if(startButton == null)
        {
            this.LogWarning("받아온 startButton가 없습니다!!");
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
            this.LogWarning("받아온 ButtonClickedCallback 이벤트가 없습니다!!");
            hasData = false;
        }

        if ((RemoveButtonClickedCallback == null))
        {
            this.LogWarning("받아온 RemoveButtonClickedCallback 이벤트가 없습니다!!");
            hasData = false;
        }

        if (hasData == false)
        {
            this.ActiveFalse();
            return false;
        }

        return true;
    }

    private void SetSprite(Sprite backgroundSprite, Sprite buttonSprite, Sprite removeSprite)
    {
        Image_Background.sprite = backgroundSprite;
        Image_Button.sprite = buttonSprite;
        Image_RemoveButton.sprite = removeSprite;
    }

    private void SetFont(TMP_FontAsset buttonFont)
    {
        Text_Button.font = buttonFont;

        Text_DataName.font = buttonFont;

        Text_DayTitle.font = buttonFont;
        Text_CoinTitle.font = buttonFont;
        Text_StaminaTitle.font = buttonFont;

        Text_DayData.font = buttonFont;
        Text_CoinData.font = buttonFont;
        Text_StaminaData.font = buttonFont;
    }

    private void SetText(string dayTitle, string CoinTitle, string staminaTitle, string startButton, PlayerModel playerModel)
    {
        if (!string.IsNullOrEmpty(playerModel.Ending))
        {
            Text_DayTitle.gameObject.SetActive(false);
        }
        else
        {
            Text_DayTitle.gameObject.SetActive(true);
            Text_DayTitle.text = dayTitle;
        }
        
        Text_CoinTitle.text = CoinTitle;
        Text_StaminaTitle.text = staminaTitle;

        Text_Button.text = startButton;
    }

    public void SetSaveData(string slotname, PlayerModel playerModel)
    {
        Text_DataName.text = slotname;

        if (!string.IsNullOrEmpty(playerModel.Ending))
        {
            if (playerModel.Ending.Contains("Happy"))
            {
                Text_DayData.text = "Happy Ending";
            }
            else if (playerModel.Ending.Contains("Bad"))
            {
                Text_DayData.text = "Bad Ending";
            }
        }
        else
        {
            Text_DayData.text = playerModel.Day.ToString();
        }

        Text_CoinData.text = playerModel.Coin.ToString();
        Text_StaminaData.text = playerModel.Stamina.ToString();
    }

    private void SetButton(string index, Action<string> buttonCallback)
    {
        OnButtonClicked = buttonCallback;
        _index = index;

        BindButtonEvent();
    }

    private void SetRemoveButton(Action<string, SaveDataButton> removeButtonCallback)
    {
        OnRemoveButtonClicked = removeButtonCallback;

        BindRemoveButtonEvent();
    }

    private void BindButtonEvent()
    {
        Button_This.onClick.RemoveAllListeners();
        Button_This.onClick.AddListener(InvokeButtonClicked);
    }

    private void BindRemoveButtonEvent()
    {
        Button_Remove.onClick.RemoveAllListeners();
        Button_Remove.onClick.AddListener(InvokeRemoveButtonClicked);
    }
    private void InvokeButtonClicked()
    {
        OnButtonClicked?.Invoke(_index);
    }

    private void InvokeRemoveButtonClicked()
    {
        OnRemoveButtonClicked?.Invoke(_index, this);
    }

    public void RequestDelete()
    {
        UIEffectUtil.AnimateAndDestroy(this.GetComponent<RectTransform>(), 0.5f);
    }
}
