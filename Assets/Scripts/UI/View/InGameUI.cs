using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : BaseUI<InGameUI>
{
    [SerializeField] private Image Image_StaminaGaugeBackground;
    [SerializeField] private Image Image_StaminaGauge;

    [SerializeField] private Transform Transform_DayGaugeButton;

    private DayGaugeButton _dayGaugeButton;

    public void SetStaminaGaugeImage(Sprite staminaGaugeBackgroundSprite, Sprite staminaGaugeSprite)
    {
        Image_StaminaGaugeBackground.sprite = staminaGaugeBackgroundSprite;
        Image_StaminaGauge.sprite = staminaGaugeSprite;
    }

    public void CreateDayGaugeButton(GameObject dayGaugeButtonPrefab, Sprite backgroundSprite, Sprite dayGaugeBackgroundSprite, Sprite dayGaugeSprite, int day, TMP_FontAsset buttonFontAsset, Action buttonCallback)
    {
        if (_dayGaugeButton != null)
        {
            return;
        }

        if (this.InstantiateAndGetComponent(dayGaugeButtonPrefab, Transform_DayGaugeButton, out DayGaugeButton dayGaugeButton) == false)
        {
            return;
        }

        if (dayGaugeButton.SetButtonData(backgroundSprite, dayGaugeBackgroundSprite, dayGaugeSprite, day, buttonFontAsset, buttonCallback) == false)
        {
            Destroy(dayGaugeButton.gameObject);
            return;
        }

        _dayGaugeButton = dayGaugeButton;
    }

    public void SetDayGaugeButtonDayGauge(float dayTime)
    {
        if(_dayGaugeButton == null)
        {
            return; 
        }

        _dayGaugeButton.ChangeGauge(dayTime);
    }

    public void SetDayGaugeButtonDay(int day)
    {
        if (_dayGaugeButton == null)
        {
            return;
        }

        _dayGaugeButton.ChangeDay(day);
    }
}
