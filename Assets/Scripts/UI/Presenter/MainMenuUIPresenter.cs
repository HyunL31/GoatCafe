using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainMenuUIPresenter : BasePresenter<MainMenuUIData, MainMenuUIPresenter, MainMenuUI>
{
    public override UIType UIType_This { get; } = UIType.MainMenuUI;

    private MainMenuUI _mainMenuUI;
    private MainMenuUIData _mainMenuData;

    public override void InitUI(MainMenuUIData mainMenuUIdata, MainMenuUI mainMenuUI)
    {
        _mainMenuUI = mainMenuUI;
        _mainMenuData = mainMenuUIdata;

        SetUI().Forget();
    }

    protected async override UniTaskVoid SetUI()
    {
        LoadUIData();
        await LoadAssetAsync();
        SubscribeEvents();
        _mainMenuUI.ActiveTrue();
    }

    protected override async UniTask LoadAssetAsync()
    {
        var (prefab_menuButtons, sprite_startButtonSprite, sprite_gameOptionButtonSprite, sprite_exitButtonSprite) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadPrefabAsync(_mainMenuData.MainMenuButtonPath),
            LoadUtil.Async.LoadSpriteAsync(_mainMenuData.StartButtonSpritePath),
            LoadUtil.Async.LoadSpriteAsync(_mainMenuData.GameOptionButtonSpritePath),
            LoadUtil.Async.LoadSpriteAsync(_mainMenuData.ExitGameButtonSpritePath)
            );

        _mainMenuUI.SetUIAsset(prefab_menuButtons, sprite_startButtonSprite, sprite_gameOptionButtonSprite, sprite_exitButtonSprite);
    }

    protected override void LoadUIData()
    {
        string startButtonText = _mainMenuData.StartButtonText;
        string gameOptionButtonText = _mainMenuData.GameOptionButtonText;
        string exitGameButtonText = _mainMenuData.ExitGameButtonText;

        _mainMenuUI.SetUIData(startButtonText, gameOptionButtonText, exitGameButtonText);
    }

    protected override void SubscribeEvents()
    {

    }

    protected override void UnsubscribeEvents()
    {

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
        UnsubscribeEvents();
        UIManager.Instance.CloseUI(UIType_This);
    }
}
