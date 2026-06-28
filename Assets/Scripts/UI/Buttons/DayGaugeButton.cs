using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayGaugeButton : BaseButton
{
    [SerializeField] private Image Image_DayGaugeBackground;
    [SerializeField] private Image Image_DayGauge;

    private Action OnButtonClicked;

    private string _day;
    private string _dayPhase;

    private void OnEnable()
    {
        GameManager.Instance.OnDayPhaseChanged += SetDayPhase;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnDayPhaseChanged -= SetDayPhase;
    }

    public bool SetButtonData(Sprite backgroundSprite, Sprite dayGaugeBackgroundSprite, Sprite dayGaugeSprite, int day, TMP_FontAsset buttonFontAsset, Action buttonCallback)
    {
        if (ComponentCheck() == false)
        {
            return false;
        }

        if (DataCheck(backgroundSprite, dayGaugeBackgroundSprite, dayGaugeSprite, buttonFontAsset, buttonCallback) == false)
        {
            return false;
        }

        SetSprite(backgroundSprite, dayGaugeBackgroundSprite, dayGaugeSprite);
        SetButton(buttonCallback);

        if (IsText)
        {
            SetFont(buttonFontAsset);
            SetText(day.ToString());
        }

        this.ActiveTrue();

        _dayPhase = "낮";
        SetButtonText();

        return true;
    }

    public void ChangeGauge(float dayTime)
    {
        Image_DayGauge.fillAmount = dayTime;
    }

    public void ChangeDay(int day)
    {
        SetText(day.ToString());
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

        if (Image_DayGaugeBackground == null)
        {
            this.LogError("Image_DayGaugeBackground 컴포넌트가 없습니다!!");
            hasComponent = false;
        }

        if (Image_DayGauge == null)
        {
            this.LogError("Image_DayGauge 컴포넌트가 없습니다!!");
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

    private bool DataCheck(Sprite backgroundSprite, Sprite dayGaugeBackgroundSprite, Sprite dayGaugeSprite, TMP_FontAsset buttonFontAsset, Action buttonCallback)
    {
        bool hasData = true;

        if (backgroundSprite == null)
        {
            this.LogWarning("받아온 backgroundSprite가 없습니다!!");
            hasData = false;
        }

        if (dayGaugeBackgroundSprite == null)
        {
            this.LogWarning("받아온 dayGaugeBackgroundSprite가 없습니다!!");
            hasData = false;
        }

        if (dayGaugeSprite == null)
        {
            this.LogWarning("받아온 dayGaugeSprite가 없습니다!!");
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

    private void SetSprite(Sprite backgroundSprite, Sprite dayGaugeBackgroundSprite, Sprite dayGaugeSprite)
    {
        Image_Button.sprite = backgroundSprite;
        Image_DayGaugeBackground.sprite = dayGaugeBackgroundSprite;
        Image_DayGauge.sprite = dayGaugeSprite;
    }

    private void SetFont(TMP_FontAsset buttonFont)
    {
        Text_Button.font = buttonFont;
    }

    private void SetText(string day)
    {
        _day = day;
        SetButtonText();
    }
    
    private void SetDayPhase(DayPhase dayPhase)
    {
        switch(dayPhase)
        {
            case DayPhase.None:
                {
                    _dayPhase = "집";
                }
                break;
                case DayPhase.Day:
                {
                    _dayPhase = "낮";
                }
                break;
            case DayPhase.Night:
                {
                    _dayPhase = "밤";
                }
                break;
            default:
                {
                    _dayPhase = "낮";
                }
                break;
        }


        SetButtonText();
    }

    private void SetButtonText()
    {
        Text_Button.text = _dayPhase + "\n" + _day;
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
