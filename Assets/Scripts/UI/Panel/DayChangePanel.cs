using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayChangePanel : MonoBehaviour
{
    [SerializeField] private RectTransform RectTransform_This;
    [SerializeField] private Image Image_Background;
    [SerializeField] private Image Image_Edge;
    [SerializeField] private Image Image_Title;

    [SerializeField] private TMP_Text Text_Description;

    [SerializeField] private Button Button_This;
    [SerializeField] private Image Image_Button;
    [SerializeField] private TMP_Text Text_Button;

    private Action OnButtonClicked;
    
    public void Open(Action buttonCallback)
    {
        this.ActiveTrue();
        OnButtonClicked += buttonCallback;
        BindButtonEvent();

        UIEffectUtil.SetScaleOne(RectTransform_This, 0.5f);
        GameManager.Instance.PauseGame();
    }

    public void Close()
    {
        OnButtonClicked = null;

        UIEffectUtil.SetScaleZero(RectTransform_This, 0.5f);
        this.ActiveFalse();

        GameManager.Instance.ResumeGame();
    }

    private void BindButtonEvent()
    {
        Button_This.onClick.AddListener(InvokeButtonClicked);
    }

    private void InvokeButtonClicked()
    {
        OnButtonClicked?.Invoke();
    }

    public void SetPanelData(Sprite backgroundSprite, Sprite edgeSprite, Sprite titleSprite, Sprite buttonSprite, TMP_FontAsset panelFont)
    {
        Image_Background.sprite = backgroundSprite;
        Image_Edge.sprite = edgeSprite;
        Image_Title.sprite = titleSprite;
        Image_Button.sprite = buttonSprite;

        Text_Description.font = panelFont;
        Text_Button.font = panelFont;
    }

    public void SetText(string description, string buttonText)
    {
        Text_Description.text = description;
        Text_Button.text = buttonText;
    }
}
