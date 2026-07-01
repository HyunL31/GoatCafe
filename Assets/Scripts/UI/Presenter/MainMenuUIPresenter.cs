using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MainMenuUIPresenter : BasePresenter<MainMenuUIPresenter, MainMenuUI>
{
    public override UIType UIType_This { get; } = UIType.MainMenuUI;
    
    private MainMenuUI _mainMenuUI;

    private GameObject _prefab_menuButton;

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
        SetUIData();

        _mainMenuUI.ActiveTrue();
    }

    protected async override UniTask LoadAssetAsync()
    {
        var (prefab_menuButton, sprite_startButton, sprite_gameOptionButton, spriteExitGameButton, fontAsset_menuFont) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.MainMenuUI.MenuButton),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.StartButton),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.GameOptionButton),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.MainMenu.ExitGameButton),

            LoadUtil.Async.LoadFontAssetAsync(AddressUtil.Font.BaseFont)
            );

        _prefab_menuButton = prefab_menuButton;

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

    }

    protected override void SetUIData()
    {
        _mainMenuUI.SetStartButton(_prefab_menuButton, _sprite_startButton, _text_startButton, _fontAsset_menuFont, OnClick_StartGame);
        _mainMenuUI.SetGameOptionButton(_prefab_menuButton, _sprite_gameOptionButton, _text_gameOptionButton, _fontAsset_menuFont, OnClick_GameOption);
        _mainMenuUI.SetExitGameButton(_prefab_menuButton, _sprite_exitGameButton, _text_exitButton, _fontAsset_menuFont, OnClick_ExitGame);
    }

    private void OnClick_StartGame()
    {
        UIManager.Instance.OpenSaveSlotPopup(On_UIExit);
    }

    private void OnClick_GameOption()
    {
        UIManager.Instance.OpenGameOptionUI();
    }

    private void OnClick_ExitGame()
    {

    }

    private void On_UIExit()
    {
        UIManager.Instance.CloseUI(UIType_This);
    }
}
