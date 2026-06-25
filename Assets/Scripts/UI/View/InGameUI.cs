using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : BaseUI<InGameUI>
{
    [SerializeField] private Image Image_StaminaGaugeBackground;
    [SerializeField] private Image Image_StaminaGauge;

    [SerializeField] private Transform Transform_DayGaugeButton;
    [SerializeField] private Transform Transform_DayChangePanel;

    private DayGaugeButton _dayGaugeButton;
    private DayChangePanel _dayChangePanel;

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

    public void CreateDayChangePanel(GameObject dayChangePanelPrefab, Sprite dayChangePanelBackgroundSprite, Sprite dayChangePanelEdgeSprite, Sprite dayChangePanelTitleSprite, Sprite dayChangePanelButtonSprite, TMP_FontAsset textFont)
    {
        if (_dayChangePanel != null)
        {
            return;
        }

        if (this.InstantiateAndGetComponent(dayChangePanelPrefab, Transform_DayChangePanel, out DayChangePanel dayChangePanel) == false)
        {
            return;
        }

        dayChangePanel.SetPanelData(dayChangePanelBackgroundSprite, dayChangePanelEdgeSprite, dayChangePanelTitleSprite, dayChangePanelButtonSprite, textFont);

        _dayChangePanel = dayChangePanel;
        _dayChangePanel.Close();
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

    public void OpenDayPanel(string description, string text)
    {
        if(_dayChangePanel == null)
        {
            return;
        }

        _dayChangePanel.SetText(description, text);
        _dayChangePanel.Open(_dayChangePanel.Close);
    }

    public void OpenNightPanel(string description, string text)
    {

        if (_dayChangePanel == null)
        {
            return;
        }

        _dayChangePanel.SetText(description,text);
        _dayChangePanel.Open(_dayChangePanel.Close);
    }
}
