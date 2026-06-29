using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class InGamePresenter : BasePresenter<InGamePresenter, InGameUI>
{
    public override UIType UIType_This { get; } = UIType.InGameUI;

    private InGameUI _inGameUI;

    private GameObject _prefab_dayButton;
    private GameObject _prefab_dayChangePanel;

    private Sprite _sprite_staminaGaugeBackground;
    private Sprite _sprite_staminaGauge;

    private Sprite _sprite_dayButtonBackground;
    private Sprite _sprite_dayButtonGaugeBackground;
    private Sprite _sprite_dayButtonGauge;

    private Sprite _sprite_dayChangePanelBackground;
    private Sprite _sprite_dayChangePanelTitle;
    private Sprite _sprite_dayChangePanelEdge;
    private Sprite _sprite_dayChangePanelButton;

    private TMP_FontAsset _fontAsset_font;

    public override void InitUI(InGameUI ui)
    {
        _inGameUI = ui;

        SetUI().Forget();
    }

    private void SubscribeEvent()
    {
        Debug.Log("[IngamePresenter] 구독");
        GameManager.Instance.OnDayTimeChanged += On_DayTimeChange;
        GameManager.Instance.OnDayPhaseChanged += On_DayPhaseChange;
        GameManager.Instance.OnDayChanged += On_DayChange;

        InputManager.Instance.OnEscKeyDown += On_UIExit;

        On_DayTimeChange(GameManager.Instance.RemainDayTime);
    }

    private void UnSubscribeEvent()
    {
        Debug.Log("[IngamePresenter] 구독해제");
        GameManager.Instance.OnDayTimeChanged -= On_DayTimeChange;
        GameManager.Instance.OnDayPhaseChanged -= On_DayPhaseChange;
        GameManager.Instance.OnDayChanged -= On_DayChange;

        InputManager.Instance.OnEscKeyDown -= On_UIExit;
    }

    protected async override UniTaskVoid SetUI()
    {
        await LoadAssetAsync();
        LoadData();
        SetUIData();
        SubscribeEvent();

        _inGameUI.ActiveTrue();
    }

    protected async override UniTask LoadAssetAsync()
    {
        var (prefab_dayButton, prefab_dayChangePanel,
            sprite_staminaGaugeBackground, sprite_staminaGauge,
            sprite_dayButtonBackground, sprite_dayButtonGaugeBackground, sprite_dayButtonGauge,
            sprite_dayChangePanelBackground, sprite_dayChangePanelTitle, sprite_dayChangePanelEdge, sprite_dayChangePanelButton,
            fontAsset_font) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.InGameUI.DayButton),
            LoadUtil.Async.LoadPrefabAsync (AddressUtil.Prefab.UI.InGameUI.DayChangePanel),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.StaminaGaugeBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.StaminaGauge),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayButtonBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayButtonGaugeBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayButtonGauge),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayChangePanelBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayChangePanelTitle),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayChangePanelEdge),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayChangePanelButton),

            LoadUtil.Async.LoadFontAssetAsync(AddressUtil.Font.BaseFont)
            );

        _prefab_dayButton = prefab_dayButton;
        _prefab_dayChangePanel = prefab_dayChangePanel;

        _sprite_staminaGaugeBackground = sprite_staminaGaugeBackground;
        _sprite_staminaGauge = sprite_staminaGauge;

        _sprite_dayButtonBackground = sprite_dayButtonBackground;
        _sprite_dayButtonGaugeBackground = sprite_dayButtonGaugeBackground;
        _sprite_dayButtonGauge = sprite_dayButtonGauge;

        _sprite_dayChangePanelBackground = sprite_dayChangePanelBackground;
        _sprite_dayChangePanelTitle = sprite_dayChangePanelTitle;
        _sprite_dayChangePanelEdge = sprite_dayChangePanelEdge;
        _sprite_dayChangePanelButton = sprite_dayChangePanelButton;

        _fontAsset_font = fontAsset_font;
    }

    protected override void LoadData()
    {
    }

    protected override void SetUIData()
    {
        int day = SaveManager.Instance.CurrentPlayerModel.Day;

        _inGameUI.SetStaminaGaugeImage(_sprite_staminaGaugeBackground, _sprite_staminaGauge);
        _inGameUI.CreateDayGaugeButton(_prefab_dayButton, _sprite_dayButtonBackground, _sprite_dayButtonGaugeBackground, _sprite_dayButtonGauge, day, _fontAsset_font, OnClick_DayGaugeButton);
        _inGameUI.CreateDayChangePanel(_prefab_dayChangePanel, _sprite_dayChangePanelBackground, _sprite_dayChangePanelEdge, _sprite_dayChangePanelTitle, _sprite_dayChangePanelButton, _fontAsset_font);
    }

    private void On_DayTimeChange(float remainDayTime)
    {
        _inGameUI.SetDayGaugeButtonDayGauge(1f - GameManager.Instance.DayTimeRate);
    }

    private void On_DayChange(int day)
    {

        _inGameUI.UpdataDayGauge(day);
    }

    private void OnClick_DayGaugeButton()
    {
        UIManager.Instance.OpenInGamePopup(On_UIExit);
    }

    private void On_UIExit()
    {
        UnSubscribeEvent();
        UIManager.Instance.CloseUI(UIType_This);
    }

    private void On_DayPhaseChange(DayPhase dayPhase)
    {
        switch(dayPhase)
        {
            case DayPhase.Day:
                {
                    _inGameUI.OpenDayPanel("낮이 되었습니다!!", "확인");
                }
                return;
            case DayPhase.Night:
                {
                    _inGameUI.OpenNightPanel("밤이 되었습니다!!", "확인");
                }
                return;
            default:
                {
                }
                return;
        }
    }
}
