using Cysharp.Threading.Tasks;

public class MainMenuUIPresenter : BasePresenter<MainMenuUIModel, MainMenuUIPresenter, MainMenuUIView>
{
    public override UIType UIType_This { get; } = UIType.MainMenuUI;
    private bool _isSetButton;

    protected async override UniTaskVoid SetUI()
    {
        await LoadAndSetAssetAsync();
        LoadUIData();
        SetButtonEvent();
        View.ActiveTrue();
    }

    protected override async UniTask LoadAndSetAssetAsync()
    {
        var (prefab_menuButtons, sprite_startButtonSprite, sprite_gameOptionButtonSprite, sprite_exitButtonSprite) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadPrefabAsync(Model.MainMenuButtonPrefabPath),
            LoadUtil.Async.LoadSpriteAsync(Model.StartButtonSpritePath),
            LoadUtil.Async.LoadSpriteAsync(Model.GameOptionButtonSpritePath),
            LoadUtil.Async.LoadSpriteAsync(Model.ExitGameButtonSpritePath)
            );

        bool isSetButton = View.SetButtonAsset(prefab_menuButtons, sprite_startButtonSprite, sprite_gameOptionButtonSprite, sprite_exitButtonSprite);

        _isSetButton = isSetButton;
    }

    protected override void LoadUIData()
    {
        string startButtonText = Model.StartButtonText;
        string gameOptionButtonText = Model.GameOptionButtonText;
        string exitGameButtonText = Model.ExitGameButtonText;

        View.SetButtonData(startButtonText, gameOptionButtonText, exitGameButtonText);
    }

    private void SetButtonEvent()
    {
        View.SubscribeButtonEvent(OnClick_StartGame, OnClick_GameOption, OnClick_ExitGame);
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
        GameManager.Instance.QuitGame();
    }

    private void On_UIExit()
    {
        View.UnsubscribeButtonEvent();
        UIManager.Instance.CloseUI(UIType_This);
    }
}
