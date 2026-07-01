using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class InGamePopupPresenter : BasePresenter<InGamePopupPresenter, InGamePopup>
{
    public override UIType UIType_This { get; } = UIType.InGamePopup;

    private InGamePopup _inGamePopup;

    private GameObject _prefab_menuButton;

    private Sprite _sprite_background;

    private Sprite _sprite_tutorialButton;
    private Sprite _sprite_gameOptionButton;
    private Sprite _sprite_returnMainMenuButton;

    private TMP_FontAsset _fontAsset_menuFont;

    private string _text_tutorialButton;
    private string _text_gameOptionButton;
    private string _text_returnMainMenuButton;

    public event Action OnReturnMainMenuClicked;

    public override void InitUI(InGamePopup ui)
    {
        _inGamePopup = ui;

        GameManager.Instance.PauseGame();
        SetUI().Forget();
    }
    private void UnSubscribeEvent()
    {
        OnReturnMainMenuClicked = null;
    }
    public void InitEvent(Action closeInGameUICallback)
    {
        OnReturnMainMenuClicked += closeInGameUICallback;
    }

    protected async override UniTaskVoid SetUI()
    {
        await LoadAssetAsync();
        LoadData();
        SetUIData();

        _inGamePopup.ActiveTrue();
    }

    protected async override UniTask LoadAssetAsync()
    {
        var(prefab_menuButton,
            sprite_background,
            sprite_tutorialButton, sprite_gameOptionButton, sprite_returnMainMenuButton,
            fontAsset_menuFont) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.InGamePopup.MenuButton),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGamePopup.Background),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGamePopup.TutorialButton),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGamePopup.GameOptionButton),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGamePopup.ReturnMainMenuButton),
            
            LoadUtil.Async.LoadFontAssetAsync(AddressUtil.Font.BaseFont)
            );

        _prefab_menuButton = prefab_menuButton;

        _sprite_background = sprite_background;

        _sprite_tutorialButton = sprite_tutorialButton;
        _sprite_gameOptionButton = sprite_gameOptionButton;
        _sprite_returnMainMenuButton = sprite_returnMainMenuButton;

        _fontAsset_menuFont = fontAsset_menuFont;
    }

    protected override void LoadData()
    {
        string dataId = DataUtil.UIData.InGamePopup.Key.Main;

        if (UIDataManager.Instance.InGamePopupDataList.TryGetValue(dataId, out InGamePopupData inGamePopupData) == false)
        {
            this.LogError($"{dataId}에 알맞는 InGamePopupData가 없습니다!!");
            return;
        }

        _text_tutorialButton = inGamePopupData.TutorialButton;
        _text_gameOptionButton = inGamePopupData.GameOptionButton;
        _text_returnMainMenuButton = inGamePopupData.ReturnMainMenuButton;
    }

    protected override void SetUIData()
    {
        _inGamePopup.SetBackgroundImageAndAction(_sprite_background, OnClick_Background);

        _inGamePopup.SetTutorialButton(_prefab_menuButton, _sprite_tutorialButton, _text_tutorialButton, _fontAsset_menuFont, OnClick_TutorialButton);
        _inGamePopup.SetGameOptionButton(_prefab_menuButton, _sprite_gameOptionButton, _text_gameOptionButton, _fontAsset_menuFont, OnClick_GameOptionButton);
        _inGamePopup.SetReturnMainMenuButton(_prefab_menuButton, _sprite_returnMainMenuButton, _text_returnMainMenuButton, _fontAsset_menuFont, OnClick_ReturnMainMenuButton);
    }

    private void OnClick_Background()
    {
        GameManager.Instance.ResumeGame();
        UIManager.Instance.CloseUI(UIType_This);
        UnSubscribeEvent();
    }

    private void OnClick_TutorialButton()
    {
        UIManager.Instance.CloseUI(UIType_This);
        UIManager.Instance.OpenTutorialPopup();
    }

    private void OnClick_GameOptionButton()
    {
        UIManager.Instance.CloseUI(UIType_This);
        UIManager.Instance.OpenGameOptionUI();
    }

    private void OnClick_ReturnMainMenuButton()
    {
        GameManager.Instance.OnCleanSpawn?.Invoke();
        GameManager.Instance.OnInitializeGoat?.Invoke();
        GameManager.Instance.ReadyGame();
        UIManager.Instance.CloseUI(UIType_This);
        OnReturnMainMenuClicked?.Invoke();
        UnSubscribeEvent();
        UIManager.Instance.OpenMainMenuUI();
        StoreManager.Instance.ResetAllStore();
    }
}