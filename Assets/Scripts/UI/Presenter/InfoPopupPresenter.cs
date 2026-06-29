using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class InfoPopupPresenter : BasePresenter<InfoPopupPresenter, InfoPopup>
{
    public override UIType UIType_This { get; } = UIType.InfoPopup;

    private InfoPopup _infoPopup;

    private int _coin;
    private int _stolenItemCount;
    private List<string> _purchasedItemNames;
    private List<string> _equippedItemNames;

    public override void InitUI(InfoPopup ui)
    {
        _infoPopup = ui;
        SetUI().Forget();
    }

    protected async override UniTaskVoid SetUI()
    {
        LoadData();
        SetUIData();
        SubscribeEvent();

        _infoPopup.ActiveTrue();
    }

    private void SubscribeEvent()
    {
        _infoPopup.OnExitButtonClicked += OnClick_ExitButton;
    }

    private void UnsubscribeEvent()
    {
        _infoPopup.OnExitButtonClicked -= OnClick_ExitButton;
    }

    protected override void LoadData()
    {
        PlayerModel playerModel = SaveManager.Instance.CurrentPlayerModel;

        _coin = playerModel.Coin;
        _stolenItemCount = playerModel.StolenItemCount;
        _purchasedItemNames = playerModel.PurchasedItemNames;
        _equippedItemNames = playerModel.EquippedItemNames;
    }

    protected override void SetUIData()
    {
        _infoPopup.SetUIData(_coin, _stolenItemCount, _purchasedItemNames, _equippedItemNames);
    }

    private void OnClick_ExitButton()
    {
        UIManager.Instance.CloseUI(UIType_This);
        UnsubscribeEvent();
    }
}
