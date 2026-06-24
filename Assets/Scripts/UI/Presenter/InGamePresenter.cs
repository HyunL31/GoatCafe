using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class InGamePresenter : BasePresenter<InGamePresenter, InGameUI>
{
    public override UIType UIType_This { get; } = UIType.InGameUI;

    private InGameUI _inGameUI;

    private GameObject _prefab_dayButton;

    private Sprite _sprite_staminaGaugeBackground;
    private Sprite _sprite_staminaGauge;

    private Sprite _sprite_dayButtonBackground;
    private Sprite _sprite_dayButtonGaugeBackground;
    private Sprite _sprite_dayButtonGauge;

    private TMP_FontAsset _fontAsset_font;

    public override void InitUI(InGameUI ui)
    {
        _inGameUI = ui;

        SetUI().Forget();
    }

    private void SubscribeEvent()
    {
        GameManager.Instance.OnDayTimeChanged += On_DayTimeChange;
        // [TODO] 스테미나 변경 이벤트 열어주면 구독

        On_DayTimeChange(GameManager.Instance.RemainDayTime);
    }

    private void UnSubscribeEvent()
    {
        GameManager.Instance.OnDayTimeChanged -= On_DayTimeChange;
        // [TODO] 스테미나 변경 이벤트 열어주면 구독해제
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
        var (prefab_dayButton,
            sprite_staminaGaugeBackground, sprite_staminaGauge,
            sprite_dayButtonBackground, sprite_dayButtonGaugeBackground, sprite_dayButtonGauge,
            fontAsset_font) = await UniTask.WhenAll
            (
            LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.InGameUI.DayButton),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.StaminaGaugeBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.StaminaGauge),

            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayButtonBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayButtonGaugeBackground),
            LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InGameUI.DayButtonGauge),

            LoadUtil.Async.LoadFontAssetAsync(AddressUtil.Font.BaseFont)
            );

        _prefab_dayButton = prefab_dayButton;

        _sprite_staminaGaugeBackground = sprite_staminaGaugeBackground;
        _sprite_staminaGauge = sprite_staminaGauge;

        _sprite_dayButtonBackground = sprite_dayButtonBackground;
        _sprite_dayButtonGaugeBackground = sprite_dayButtonGaugeBackground;
        _sprite_dayButtonGauge = sprite_dayButtonGauge;

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
    }

    private void On_DayTimeChange(float remainDayTime)
    {
        _inGameUI.SetDayGaugeButtonDayGauge(1f - GameManager.Instance.DayTimeRate);
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
}
