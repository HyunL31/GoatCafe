using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SaveDataSlotPopupPresenter : BasePresenter<SaveDataSlotPopupPresenter, MainUI>
{
    public override UIType UIType_This { get; } = UIType.SaveSlotPopup;

    private MainUI _mainUI;

    private GameObject _prefab_loadButton;

    private Sprite _sprite_background;
    private Sprite _sprite_titleImage;
    private Sprite _sprite_saveButtons;

    private TMP_FontAsset _fontAsset_menuFont;

    public override void InitUI(MainUI ui)
    {
        _mainUI = ui;
    }

    protected async override UniTaskVoid SetUI()
    {
        await LoadAssetAsync();
        LoadData();


    }

    protected async override UniTask LoadAssetAsync()
    {
        var (sprite_background, sprite_titleImage, sprite_saveButton, fontAsset_menuFont) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.Background),
            LoadUtil.Async.LoadSpriteAsync (AddressUtil.Sprite.UI.SaveSlotPopup.TitleImage),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.SaveSlotPopup.SaveButton),
            LoadUtil.Async.LoadFontAssetAsync(AddressUtil.Font.BaseFont)
            );

        _sprite_background = sprite_background;
        _sprite_titleImage = sprite_titleImage;
        _sprite_saveButtons = sprite_saveButton;
        _fontAsset_menuFont = fontAsset_menuFont;

    }

    protected override void LoadData()
    {
        for (int i = 0; i < GameManager.Instance.SlotIndex.Count; i++)
        {
            PlayerModel playerModel = SaveManager.Instance.RequestLoadData(GameManager.Instance.SlotIndex[i]);
        
            
        
        
        }


    }

}
