using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataSlotPopup : BaseUI<SaveDataSlotPopup>
{
    [SerializeField] private Image Image_BackgroundEdge;
    [SerializeField] private Image Image_Background;
    [SerializeField] private Image Image_BackgroundTitle;

    [SerializeField] private Image Image_SaveSlotEdge;
    [SerializeField] private Image Image_SaveSlotBackground;
    [SerializeField] private Image Image_SaveSlotTitle;

    [SerializeField] private Transform Transform_SaveDataSlot;
    [SerializeField] private Transform Transform_SelectPanel;
    [SerializeField] private Transform Transform_NameSetPanel;
    [SerializeField] private Transform Transform_ExitButton;

    private SelectPanel _selectPanel;
    private NameSetPanel _nameSetPanel;
    private NormalButton _exitButton;


    public void SetBackgroundImage(Sprite backgroundEdgeSprite, Sprite backgroundSprite, Sprite backgroundTitleSprite = null)
    {
        Image_BackgroundEdge.sprite = backgroundEdgeSprite;
        Image_Background.sprite = backgroundSprite;

        if (backgroundTitleSprite == null)
        {
            Image_BackgroundTitle.ActiveFalse();
        }
        else
        { 
            Image_BackgroundTitle.sprite = backgroundTitleSprite;
            Image_BackgroundTitle.ActiveTrue();
        }
    }

    public void SetSaveSlotImage(Sprite saveSlotEdgeSprite, Sprite saveSlotBackgroundSprite, Sprite saveSlotTitleSprite)
    {
        Image_SaveSlotEdge.sprite = saveSlotEdgeSprite;
        Image_SaveSlotBackground.sprite = saveSlotBackgroundSprite;

        if (saveSlotTitleSprite == null)
        {
            Image_SaveSlotTitle.ActiveFalse();
        }
        else
        {
            Image_SaveSlotTitle.sprite = saveSlotTitleSprite;
            Image_SaveSlotTitle.ActiveTrue();
        }
    }

    public void CreateNewSlotButton(GameObject newDataButtonPrefab, Sprite backgroundSprite, Sprite buttonSprite, string createNewButton, TMP_FontAsset fontAsset, Action newSlotButtonCallback)
    {
        if(this.InstantiateAndGetComponent(newDataButtonPrefab, Transform_SaveDataSlot, out NewDataButton newDataButton) == false)
        {
            return;
        }

        if (newDataButton.SetButtonData(backgroundSprite, buttonSprite, createNewButton, fontAsset, newSlotButtonCallback) == false)
        {
            Destroy(newDataButton.gameObject);
        }
    }

    public void CreateSaveSlotButton(GameObject saveDataButtonPrefab, string slotname, PlayerModel playerModel, Sprite backgroundSprite, Sprite buttonSprite, string dayTitle, string goldTitle, string staminaTitle, string startButton, TMP_FontAsset fontAsset, Action<string> saveSlotButtonCallback)
    {
        if (this.InstantiateAndGetComponent(saveDataButtonPrefab, Transform_SaveDataSlot, out SaveDataButton saveDataButton) == false)
        {
            return;
        }

        if (saveDataButton.SetButtonData(backgroundSprite, buttonSprite,dayTitle, goldTitle, staminaTitle, startButton, fontAsset, slotname, playerModel, saveSlotButtonCallback) == false)
        {
            Destroy(saveDataButton.gameObject);
        }
    }
    public void CreateExitButton(GameObject exitButtonPrefab, Sprite exitButtonSprite, Action exitButtonCallback)
    {
        if(_exitButton != null)
        {
            return;
        }

        if (this.InstantiateAndGetComponent(exitButtonPrefab, Transform_ExitButton, out NormalButton exitButton) == false)
        {
            return;
        }

        if (exitButton.SetButtonData(exitButtonSprite, null, null, exitButtonCallback) == false)
        {
            Destroy(exitButton.gameObject);
            return;
        }

        _exitButton = exitButton;
    }

    public void DestroyAllSaveDataSlot()
    {
        foreach (Transform saveDataSlotTransform in Transform_SaveDataSlot)
        {
            Destroy(saveDataSlotTransform.gameObject);
        }
    }

    public void CreateSelectPanel(GameObject selectPanelPrefab, Sprite backgroundSprite, Sprite yesButtonSprite, Sprite noButtonSprite, TMP_FontAsset buttonFont)
    {
        if (_selectPanel == null)
        {
            if (this.InstantiateAndGetComponent(selectPanelPrefab, Transform_SelectPanel, out SelectPanel selectPanel) == false)
            {
                return;
            }
            _selectPanel = selectPanel;
        }

        _selectPanel.SetSelectPanel(backgroundSprite, yesButtonSprite, noButtonSprite, buttonFont);
        _selectPanel.ActiveFalse();
    }

    public void CreateNameSetPanel(GameObject nameSetPanelPrefab, Sprite backgroundSprite, Sprite inputFieldBackground, Sprite buttonSprite, Sprite exitButtonSprite, string description, string buttonText, TMP_FontAsset buttonFont)
    {
        if (_nameSetPanel == null)
        {
            if (this.InstantiateAndGetComponent(nameSetPanelPrefab, Transform_NameSetPanel, out NameSetPanel nameSetPanel) == false)
            {
                return;
            }
            _nameSetPanel = nameSetPanel;
        }

        _nameSetPanel.SetNameSetPanel(backgroundSprite, inputFieldBackground, buttonSprite, exitButtonSprite, description, buttonText, buttonFont);
        _nameSetPanel.ActiveFalse();
    }


    public void OpenNewDataSlotPanel(string titleText, string descriptionText, string yesButtonText, string noButtonText, Action onYesButtonCallback, Action onNoButtonCallback)
    {
        if(_selectPanel == null)
        {
            return;
        }

        _selectPanel.SetText(titleText, descriptionText, yesButtonText, noButtonText);

        _selectPanel.BindYesButtonEvent(onYesButtonCallback);
        _selectPanel.BindNoButtonEvent(onNoButtonCallback);

        _selectPanel.Open();
    }

    public void OpenSelectSlotPanel(string titleText, PlayerModel playerModel, string yesButtonText, string noButtonText, Action onYesButtonCallback, Action onNoButtonCallback)
    {
        if (_selectPanel == null)
        {
            return;
        }

        string descriptionText = $"날짜 : {playerModel.Day}\n 코인 : {playerModel.Coin}\n 스테미나 : {playerModel.Stamina}";

        _selectPanel.SetText(titleText, descriptionText, yesButtonText, noButtonText);

        _selectPanel.BindYesButtonEvent(onYesButtonCallback);
        _selectPanel.BindNoButtonEvent(onNoButtonCallback);

        _selectPanel.Open();
    }
    public void OpenNameSetPanel(Action<string> nameSetCallback)
    {
        if (_nameSetPanel == null)
        {
            return;
        }

        _nameSetPanel.BindButtonEvent(nameSetCallback);
        _nameSetPanel.Open();
    }
}