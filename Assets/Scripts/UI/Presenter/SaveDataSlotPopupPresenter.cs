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
    private GameObject _prefab_selectPanel;
    private GameObject _prefab_nameSetPanel;
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

    private Sprite _sprite_removeButton;

    private Sprite _sprite_nameSetPanelBackground;
    private Sprite _sprite_nameSetPanelInputFieldBackground;
    private Sprite _sprite_nameSetPanelSelectButton;
    private Sprite _sprite_nameSetPanelExitButton;

    private Sprite _sprite_selectPanelBackground;
    private Sprite _sprite_selectPanelYesButton;
    private Sprite _sprite_selectPanelNoButton;

    private Sprite _sprite_exitPopupButton;

    private TMP_FontAsset _fontAsset_menuFont;

    private Action OnSaveDataSlotSelected;

    private List<(string, PlayerModel)> _slotDataList = new();

    private string _selectSlotName;
    private SaveDataButton _selectSlotData;

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
        var(prefab_newDataButton, prefab_saveDataButton, prefab_exitPopupButton, prefab_selectPanel, prefab_nameSetPanel,
            sprite_backgroundEdge, sprite_background, sprite_backgroundTitle,
            sprite_saveSlotEdge, sprite_saveSlotBackground, sprite_saveSlotTitle,
            fontAsset_menuFont) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.SaveSlotPopup.NewDataButton),
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.SaveSlotPopup.SaveDataButton),
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.SaveSlotPopup.ExitPopupButton),
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.SaveSlotPopup.SelectPanel),
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.SaveSlotPopup.NameSetPanel),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.BackgroundEdge),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.Background),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.BackgroundTitle),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveSlotEdge),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveSlotBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveSlotTitle),

            LoadUtil.Async.LoadFontAssetAsync(AddressUtil.Font.BaseFont)
            );

        var (sprite_newSlotButtonBackground,
            sprite_saveButtonBlueBackground, sprite_saveButtonGreenBackground, sprite_saveButtonRedBackground,
            sprite_saveButtonBlue, sprite_saveButtonGreen, sprite_saveButtonRed,
            sprite_selectPanelBackground, sprite_selectPanelYesButton, sprite_selectPanelNoButton,
            sprite_nameSetPanelBackgroud, sprite_nameSetPanelInputFieldBackground, sprite_nameSetPanelSelectButton, sprite_nameSetPanelExitButton,
            sprite_exitPopupButton
            ) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.NewSlotButtonBackground),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonBlueBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonGreenBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonRedBackground),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonBlue),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonGreen),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButtonRed),


            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SelectPanelBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SelectPanelYesButton),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SelectPanelNoButton),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.NameSetPanelBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.NameSetPanelInputFieldBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.NameSetPanelSelectButton),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.NameSetPanelExitButton),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.ExitPopupButton)
            );

        var sprite_slotRemoveButton = await LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SlotRemoveButton);

        _sprite_removeButton = sprite_slotRemoveButton;

        _prefab_newDataButton = prefab_newDataButton;
        _prefab_saveDataButton = prefab_saveDataButton;
        _prefab_exitPopupButton = prefab_exitPopupButton;
        _prefab_selectPanel = prefab_selectPanel;
        _prefab_nameSetPanel = prefab_nameSetPanel;

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

        _sprite_selectPanelBackground = sprite_selectPanelBackground;
        _sprite_selectPanelYesButton = sprite_selectPanelYesButton;
        _sprite_selectPanelNoButton = sprite_selectPanelNoButton;

        _sprite_nameSetPanelBackground = sprite_nameSetPanelBackgroud;
        _sprite_nameSetPanelInputFieldBackground = sprite_nameSetPanelInputFieldBackground;
        _sprite_nameSetPanelSelectButton = sprite_nameSetPanelSelectButton;
        _sprite_nameSetPanelExitButton = sprite_nameSetPanelExitButton;

        _sprite_exitPopupButton = sprite_exitPopupButton;

        _fontAsset_menuFont = fontAsset_menuFont;
    }

    protected override void LoadData()
    {
        _slotDataList.Clear();

        foreach (string slotName in SaveManager.Instance.SlotIndex)
        {
            PlayerModel playermodel = SaveManager.Instance.RequestLoadData(slotName);

            _slotDataList.Add((slotName, playermodel));
        }
    }

    protected override void SetUIData()
    {
        _saveDataSlotPopup.DestroyAllSaveDataSlot();

        _saveDataSlotPopup.SetBackgroundImage(_sprite_backgroundEdge, _sprite_background, _sprite_backgroundTitle);
        _saveDataSlotPopup.SetSaveSlotImage(_sprite_saveSlotEdge, _sprite_saveSlotBackground, _sprite_saveSlotTitle);


        _saveDataSlotPopup.CreateSelectPanel(_prefab_selectPanel, _sprite_selectPanelBackground, _sprite_selectPanelYesButton, _sprite_selectPanelNoButton, _fontAsset_menuFont);
        _saveDataSlotPopup.CreateNameSetPanel(_prefab_nameSetPanel, _sprite_nameSetPanelBackground, _sprite_nameSetPanelInputFieldBackground, _sprite_nameSetPanelSelectButton, _sprite_nameSetPanelExitButton, "슬롯의 이름을 결정해주십시오.", "확인" , _fontAsset_menuFont);
        _saveDataSlotPopup.CreateExitButton(_prefab_exitPopupButton, _sprite_exitPopupButton, OnClick_ExitPopup);

        _saveDataSlotPopup.CreateNewSlotButton(_prefab_newDataButton, _sprite_newSlotButtonBackground, _sprite_saveButtonRed, "처음부터", _fontAsset_menuFont, OnClick_NewSlot);

        int slotCount = 0;

        foreach ((string slotIndex, PlayerModel playerModel) in _slotDataList)
        {
            (Sprite buttonBackgroundSprite, Sprite buttonSprite, Sprite removeSprite) = GetSlotSprite(slotCount);

            _saveDataSlotPopup.CreateSaveSlotButton(_prefab_saveDataButton, slotIndex, playerModel, buttonBackgroundSprite, buttonSprite, removeSprite, "날짜 :", "코인 :", "스테미너 :", "시작하기", _fontAsset_menuFont, OnClick_Slot, OnClick_RemoveSlot);

            slotCount++;
        }
    }

    private (Sprite, Sprite, Sprite) GetSlotSprite(int slotCount)
    {
        switch (slotCount % 3)
        {
            case 0:
                {
                    return (_sprite_saveButtonBlueBackground, _sprite_saveButtonBlue, _sprite_removeButton);
                }
            case 1:
                {
                    return (_sprite_saveButtonGreenBackground ,_sprite_saveButtonGreen, _sprite_removeButton);
                }
            case 2:
                {
                    return (_sprite_saveButtonRedBackground, _sprite_saveButtonRed, _sprite_removeButton);
                }
            default:
                {
                    LogError("있을 수 없는 일이 발생하였습니다?!");
                    return (null, null, null);
                }
        }
    }

    private void OnClick_NewSlot()
    {
        _saveDataSlotPopup.OpenNewDataSlotPanel("슬롯을 새로 생성하시겠습니까?", null, "예", "아니오", OnClick_NewSlotYes, null);
    }

    private void OnClick_Slot(string slotName)
    {
        PlayerModel playerModel = SaveManager.Instance.RequestLoadData(slotName);
        _selectSlotName = slotName;

        _saveDataSlotPopup.OpenSelectSlotPanel("이 슬롯을 사용하시겠습니까?", playerModel, "예", "아니오", OnClick_SlotYes, null);

    }

    private void OnClick_RemoveSlot(string slotName, SaveDataButton saveSlot)
    {
        PlayerModel playerModel = SaveManager.Instance.RequestLoadData(slotName);
        _selectSlotName = slotName;
        _selectSlotData = saveSlot;

        _saveDataSlotPopup.OpenSelectSlotPanel("이 슬롯을 삭제하시겠습니까?", playerModel, "예", "아니오", OnClick_RemoveSlotYes, null);
    }

    private void OnClick_ExitPopup()
    {
        UIManager.Instance.CloseUI(UIType_This);
    }

    private void OnClick_NewSlotYes()
    {
        _saveDataSlotPopup.OpenNameSetPanel(GameStart);
    }

    private void OnClick_SlotYes()
    {
        GameStart(_selectSlotName);
    }

    private void OnClick_RemoveSlotYes()
    {
        SaveManager.Instance.DeleteSaveData(_selectSlotName);

        int slotIndex = _slotDataList.FindIndex(item => item.Item1 == _selectSlotName);
        _slotDataList.RemoveAt(slotIndex);

        SaveManager.Instance.RemoveSlotIndex(_selectSlotName);

        _selectSlotData.RequestDelete();
    }

    private void GameStart(string slotName)
    {
        SaveManager.Instance.LoadOrCreatePlayerData(slotName);
        UIManager.Instance.CloseUI(UIType_This);
        OnSaveDataSlotSelected?.Invoke();

        UIManager.Instance.OpenDialogueUI();
        //UIManager.Instance.OpenInGameUI();
    }
}
