using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameSetPanel : MonoBehaviour
{
    [SerializeField] private Image Image_Background;

    [SerializeField] private Image Image_InputFieldBackground;
    [SerializeField] private TMP_Text Text_Description;
    [SerializeField] private TMP_InputField InputField_Name;

    [SerializeField] private Button Button_Select;
    [SerializeField] private Image Image_SelectButton;
    [SerializeField] private TMP_Text Text_SelectButton;

    [SerializeField] private Button Button_Exit;
    [SerializeField] private Image Image_ExitButton;

    private TMP_FontAsset _paneFontAsset;

    private Action<string> OnSelectButtonClicked;

    public void SetNameSetPanel(Sprite backgroundSprite, Sprite inputFieldBackground, Sprite buttonSprite, Sprite exitButtonSprite, string description, string buttonText, TMP_FontAsset panelFontAsset)
    {
        Image_Background.sprite = backgroundSprite;
        _paneFontAsset = panelFontAsset;

        Image_InputFieldBackground.sprite = inputFieldBackground;
        Image_SelectButton.sprite = buttonSprite;

        Text_Description.text = description;
        Text_Description.font = _paneFontAsset;

        Text_SelectButton.text = buttonText;
        Text_SelectButton.font = _paneFontAsset;

        Image_ExitButton.sprite = exitButtonSprite;

        
    }

    public void Open()
    {
        this.ActiveTrue();
        InputField_Name.fontAsset = _paneFontAsset;
        InputField_Name.text = "";
    }

    private void Close()
    {
        this.ActiveFalse();
    }

    public void BindButtonEvent(Action<string> selectButtonCallback)
    {
        OnSelectButtonClicked = selectButtonCallback;

        Button_Select.onClick.RemoveAllListeners();
        Button_Select.onClick.AddListener(InvokeSelectButtonClicked);

        Button_Exit.onClick.RemoveAllListeners();
        Button_Exit.onClick.AddListener(Close);
    }

    private void InvokeSelectButtonClicked()
    {
        string indexName = InputField_Name.text;

        Close();
        OnSelectButtonClicked?.Invoke(indexName);
    }
}