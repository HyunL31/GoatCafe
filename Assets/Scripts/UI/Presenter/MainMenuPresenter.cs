using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class MainMenuUIPresenter : BasePresenter<MainMenuUIPresenter, MainMenuUI>
{
    public override UIType UIType_This { get; } = UIType.MainMenuUI;
    
    private MainMenuUI _mainMenuUI;

    private GameObject _prefab_menuButton;

    private Sprite _sprite_background;
    private Sprite _sprite_title;

    private Sprite _sprite_menuSlotEdge;
    private Sprite _sprite_menuSlotBackground;
    private Sprite _sprite_menuSlotTitle;

    private Sprite _sprite_startButton;
    private Sprite _sprite_gameOptionButton;
    private Sprite _sprite_exitGameButton;

    private TMP_FontAsset _fontAsset_menuFont;

    private string _text_startButton;
    private string _text_gameOptionButton;
    private string _text_exitButton;

    public override void InitUI(MainMenuUI mainMenuUI)
    {
        _mainMenuUI = mainMenuUI;

        SetUI().Forget();
    }

    protected async override UniTaskVoid SetUI()
    {
        await LoadAssetAsync();
        LoadData();

        _mainMenuUI.ActiveTrue();
    }

    protected async override UniTask LoadAssetAsync()
    {
        var (prefab_menuButton, sprite_background, sprite_title, sprite_menuSlotEdge, sprite_menuSlotBackground, sprite_menuSlotTitle, sprite_startButton, sprite_gameOptionButton, spriteExitGameButton, fontAsset_menuFont) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.MainMenuUI.MenuButton),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.Background),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.Title),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.MenuSlotEdge),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.MenuSlotBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.MenuSlotTitle),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.StartButton),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.GameOptionButton),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.ExitGameButton),

            LoadUtil.Async.LoadFontAssetAsync(AddressUtil.Font.BaseFont)
            );

        _prefab_menuButton = prefab_menuButton;

        _sprite_background = sprite_background;

        _sprite_title = sprite_title;

        _sprite_menuSlotEdge = sprite_menuSlotEdge;
        _sprite_menuSlotBackground = sprite_menuSlotBackground;
        _sprite_menuSlotTitle = sprite_menuSlotTitle;

        _sprite_startButton = sprite_startButton;
        _sprite_gameOptionButton = sprite_gameOptionButton;
        _sprite_exitGameButton = spriteExitGameButton;

        _fontAsset_menuFont = fontAsset_menuFont;
    }

    protected override void LoadData()
    {
        string dataId = DataUtil.UIData.MainMenuUI.Key.Main;

        if(UIDataManager.Instance.MainMenuUIDataList.TryGetValue(dataId, out MainMenuUIData mainMenuUIData) == false)
        {
            this.LogError($"{dataId}에 알맞는 MainMenuUIData가 없습니다!!");
            return;
        }

        _text_startButton = mainMenuUIData.StartButton;
        _text_gameOptionButton = mainMenuUIData.GameOptionButton;
        _text_exitButton = mainMenuUIData.ExitGameButton;

        SetUIData();
    }

    protected override void SetUIData()
    {
        _mainMenuUI.SetBackgroundImage(_sprite_background);

        _mainMenuUI.SetTitleImage(_sprite_title);

        _mainMenuUI.SetMenuSlotImage(_sprite_menuSlotEdge, _sprite_menuSlotBackground, _sprite_menuSlotTitle);

        _mainMenuUI.SetStartButton(_prefab_menuButton, _sprite_startButton, _text_startButton, _fontAsset_menuFont, OnClick_StartGame);
        _mainMenuUI.SetGameOptionButton(_prefab_menuButton, _sprite_gameOptionButton, _text_gameOptionButton, _fontAsset_menuFont, OnClick_GameOption);
        _mainMenuUI.SetExitGameButton(_prefab_menuButton, _sprite_exitGameButton, _text_exitButton, _fontAsset_menuFont, OnClick_ExitGame);

        _mainMenuUI.ActiveTrue();
    }

    private void OnClick_StartGame()
    {
        UIManager.Instance.OpenSaveSlotPopup(On_UIExit);
    }

    private void OnClick_GameOption()
    {
        UIManager.Instance.OpenGameOptionPopup();
    }

    private void OnClick_ExitGame()
    {

    }

    private void On_UIExit()
    {
        UIManager.Instance.CloseUI(UIType_This);
    }
}
