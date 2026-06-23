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
    [SerializeField] private Transform Transform_ExitButton;

    private bool _hasExitButton;


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

    public void CreateNewSlotButton(GameObject newDataButtonPrefab, int slotindex, Sprite backgroundSprite, Sprite buttonSprite, string createNewButton, TMP_FontAsset fontAsset, Action newSlotButtonCallback)
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

    public void CreateSaveSlotButton(GameObject saveDataButtonPrefab, int slotIndex, PlayerModel playerModel, Sprite backgroundSprite, Sprite buttonSprite, string dayTitle, string goldTitle, string staminaTitle, TMP_FontAsset fontAsset, Action<int> saveSlotButtonCallback)
    {
        if (this.InstantiateAndGetComponent(saveDataButtonPrefab, Transform_SaveDataSlot, out SaveDataButton saveDataButton) == false)
        {
            return;
        }

        if (saveDataButton.SetButtonData(backgroundSprite, buttonSprite, dayTitle, goldTitle, staminaTitle, "시작하기", fontAsset, slotIndex, playerModel, saveSlotButtonCallback) == false)
        {
            Destroy(saveDataButton.gameObject);
        }
    }
    public void CreateExitButton(GameObject exitButtonPrefab, Sprite exitButtonSprite, Action exitButtonCallback)
    {
        if(_hasExitButton)
        {
            return;
        }

        if (this.InstantiateAndGetComponent(exitButtonPrefab, Transform_ExitButton, out NormalButton normalButton) == false)
        {
            return;
        }

        if (normalButton.SetButtonData(exitButtonSprite, null, null, exitButtonCallback) == false)
        {
            Destroy(normalButton.gameObject);
        }

        _hasExitButton = true;
    }

    public void DestroyAllSaveDataSlot()
    {
        foreach (Transform saveDataSlotTransform in Transform_SaveDataSlot)
        {
            Destroy(saveDataSlotTransform.gameObject);
        }
    }
}