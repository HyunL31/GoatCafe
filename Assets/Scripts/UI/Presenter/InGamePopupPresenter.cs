using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class InGamePopupPresenter : BasePresenter<InGamePopupPresenter, InGamePopup>
{
    public override UIType UIType_This { get; } = UIType.InGamePopup;

    private InGamePopup _inGamePopup;

    private GameObject _prefab_menuButton;

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
        SubscribeEvents();

        _inGamePopup.ActiveTrue();
    }

    private void SubscribeEvents()
    {
        _inGamePopup.OnBackgroundClicked += OnClick_Background;
    }

    protected async override UniTask LoadAssetAsync()
    {
        GameObject prefab_menuButton = await LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.InGamePopup.MenuButton);

        _prefab_menuButton = prefab_menuButton;
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
        _inGamePopup.SetTutorialButton(_prefab_menuButton, _text_tutorialButton, OnClick_TutorialButton);
        _inGamePopup.SetGameOptionButton(_prefab_menuButton, _text_gameOptionButton, OnClick_GameOptionButton);
        _inGamePopup.SetReturnMainMenuButton(_prefab_menuButton, _text_returnMainMenuButton, OnClick_ReturnMainMenuButton);
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
        UIManager.Instance.CloseUI(UIType_This);
        OnReturnMainMenuClicked?.Invoke();
        UnSubscribeEvent();
        GameManager.Instance.ReadyGame();
        UIManager.Instance.OpenMainMenuUI();
        StoreManager.Instance.ResetAllStore();
    }
}