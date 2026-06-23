using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveDataSlotPopupPresenter : BasePresenter<SaveDataSlotPopupPresenter, SaveDataSlotPopup>
{
    public override UIType UIType_This { get; } = UIType.SaveSlotPopup;

    private SaveDataSlotPopup _saveDataSlotPopup;

    private GameObject _prefab_newDataButton;
    private GameObject _prefab_saveDataButton;
    private GameObject _prefab_exitPopupButton;

    private Sprite _sprite_backgroundEdge;
    private Sprite _sprite_background;
    private Sprite _sprite_backgroundTitle;

    private Sprite _sprite_saveSlotEdge;
    private Sprite _sprite_saveSlotBackground;
    private Sprite _sprite_saveSlotTitle;

    private Sprite _sprite_newSlotButtonBackground;

    private Sprite _sprite_saveButtonBlueBackground;
    private Sprite _sprite_saveButtonGreenBackground;
    private Sprite _sprite_saveButtonRedBackground;

    private Sprite _sprite_saveButtonBlue;
    private Sprite _sprite_saveButtonGreen;
    private Sprite _sprite_saveButtonRed;

    private Sprite _sprite_exitPopupButton;

    private TMP_FontAsset _fontAsset_menuFont;

    private Action OnSaveDataSlotSelected;

    private List<(int index, PlayerModel model)> _slotDataList = new();

    public override void InitUI(SaveDataSlotPopup ui)
    {
        _saveDataSlotPopup = ui;
        SetUI().Forget();
    }

    public void SetAction(Action onSaveDataSlotSelected)
    {
        OnSaveDataSlotSelected = onSaveDataSlotSelected;
    }

    protected async override UniTaskVoid SetUI()
    {
        await LoadAssetAsync();
        LoadData();
        SetUIData();

        _saveDataSlotPopup.ActiveTrue();
    }

    protected async override UniTask LoadAssetAsync()
    {
        var (prefab_newDataButton, prefab_saveDataButton, prefab_exitPopupButton,
            sprite_backgroundEdge, sprite_background, sprite_backgroundTitle,
            sprite_saveSlotEdge, sprite_saveSlotBackground, sprite_saveSlotTitle) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.SaveSlotPopup.NewDataButton),
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.SaveSlotPopup.SaveDataButton),
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.SaveSlotPopup.ExitPopupButton),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.BackgroundEdge),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.Background),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.BackgroundTitle),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveSlotEdge),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveSlotBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveSlotTitle)
            );

        var (sprite_newSlotButtonBackground,
            sprite_saveButtonBlueBackground, sprite_saveButtonGreenBackground, sprite_saveButtonRedBackground,
            sprite_saveButtonBlue, sprite_saveButtonGreen, sprite_saveButtonRed,
            sprite_exitPopupButton,
            fontAsset_menuFont) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.NewSlotButtonBackground),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonBlueBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonGreenBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonRedBackground),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonBlue),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonGreen),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonRed),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.ExitPopupButton),

            LoadUtil.Async.LoadFontAssetAsync(AddressUtil.Font.BaseFont)
            );

        _prefab_newDataButton = prefab_newDataButton;
        _prefab_saveDataButton = prefab_saveDataButton;
        _prefab_exitPopupButton = prefab_exitPopupButton;

        _sprite_backgroundEdge = sprite_backgroundEdge;
        _sprite_background = sprite_background;
        _sprite_backgroundTitle = sprite_backgroundTitle;

        _sprite_saveSlotEdge = sprite_saveSlotEdge;
        _sprite_saveSlotBackground = sprite_saveSlotBackground;
        _sprite_saveSlotTitle = sprite_saveSlotTitle;

        _sprite_newSlotButtonBackground = sprite_newSlotButtonBackground;

        _sprite_saveButtonBlueBackground = sprite_saveButtonBlueBackground;
        _sprite_saveButtonGreenBackground = sprite_saveButtonGreenBackground;
        _sprite_saveButtonRedBackground = sprite_saveButtonRedBackground;

        _sprite_saveButtonBlue = sprite_saveButtonBlue;
        _sprite_saveButtonGreen = sprite_saveButtonGreen;
        _sprite_saveButtonRed = sprite_saveButtonRed;

        _sprite_exitPopupButton = sprite_exitPopupButton;

        _fontAsset_menuFont = fontAsset_menuFont;
    }

    protected override void LoadData()
    {
        _slotDataList.Clear();

        foreach (int dataIndex in GameManager.Instance.SlotIndex)
        {
            PlayerModel playermodel = SaveManager.Instance.RequestLoadData(dataIndex);

            _slotDataList.Add((dataIndex, playermodel));
        }
    }

    protected override void SetUIData()
    {
        _saveDataSlotPopup.DestroyAllSaveDataSlot();

        _saveDataSlotPopup.SetBackgroundImage(_sprite_backgroundEdge, _sprite_background, _sprite_backgroundTitle);
        _saveDataSlotPopup.SetSaveSlotImage(_sprite_saveSlotEdge, _sprite_saveSlotBackground, _sprite_saveSlotTitle);

        _saveDataSlotPopup.CreateExitButton(_prefab_exitPopupButton, _sprite_exitPopupButton, OnClick_ExitPopup);

        int emptyIndex = GameManager.Instance.GetEmptySlotIndex();

        _saveDataSlotPopup.CreateNewSlotButton(_prefab_newDataButton, emptyIndex, _sprite_newSlotButtonBackground, _sprite_saveButtonRed, "처음부터", _fontAsset_menuFont, OnClick_NewSlot);

        int slotCount = 0;

        foreach ((int index, PlayerModel playerModel) in _slotDataList)
        {
            (Sprite buttonBackgroundSprite, Sprite buttonSprite) = GetSlotSprite(slotCount);

            _saveDataSlotPopup.CreateSaveSlotButton(_prefab_saveDataButton, index, playerModel, buttonBackgroundSprite, buttonSprite, "날짜 :", "골드 :", "스테미너 :", _fontAsset_menuFont, OnClick_Slot);

            slotCount++;
        }
    }

    private (Sprite, Sprite) GetSlotSprite(int slotCount)
    {
        switch (slotCount % 3)
        {
            case 0:
                {
                    return (_sprite_saveButtonBlueBackground, _sprite_saveButtonBlue);
                }
            case 1:
                {
                    return (_sprite_saveButtonGreenBackground ,_sprite_saveButtonGreen);
                }
            case 2:
                {
                    return (_sprite_saveButtonRedBackground, _sprite_saveButtonRed);
                }
            default:
                {
                    LogError("있을 수 없는 일이 발생하였습니다?!");
                    return (null, null);
                }
        }
    }

    private void OnClick_NewSlot()
    {
        int emptyIndex = GameManager.Instance.GetEmptySlotIndex();
        GameStart(emptyIndex);
    }

    private void OnClick_Slot(int index)
    {
        GameStart(index);
    }

    private void OnClick_ExitPopup()
    {
        UIManager.Instance.CloseUI(UIType_This);
    }

    private void GameStart(int index)
    {
        GameManager.Instance.LoadOrCreatePlayerData(index);
        UIManager.Instance.CloseUI(UIType_This);
        OnSaveDataSlotSelected?.Invoke();

        UIManager.Instance.OpenInGameUI();
    }
}
