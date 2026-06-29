using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InfoPopup : BaseUI<InfoPopup>
{
    [SerializeField] private Button Button_Exit;

    [SerializeField] private TMP_Text Text_CoinTitle;
    [SerializeField] private TMP_Text Text_CoinData;

    [SerializeField] private TMP_Text Text_StolenItemCountTitle;
    [SerializeField] private TMP_Text Text_StolenItemCountData;

    [SerializeField] private TMP_Text Text_PurchasedItemTitle;
    [SerializeField] private TMP_Text Text_PurchasedItemData;

    [SerializeField] private TMP_Text Text_EquippedItemTitle;
    [SerializeField] private TMP_Text Text_EquippedItemData;

    public event Action OnExitButtonClicked;

    private void OnEnable()
    {
        BindButtonEvent();
    }

    private void BindButtonEvent()
    {
        //Button_Exit.onClick.AddListener(InvokeExitBUttonClicked);
    }

    private void OnDisable()
    {
        UnBindButtonEvent();
    }

    private void UnBindButtonEvent()
    {
        //Button_Exit.onClick.RemoveListener(InvokeExitBUttonClicked);
    }

    public void SetUIData(int coinData, int stolenItemCountData, List<string> purchasedItemData, List<string> equippedItemData)
    {
        Text_CoinData.text = coinData.ToString();
        Text_StolenItemCountData.text = stolenItemCountData.ToString();

        string purchasedItems = GetItemList(purchasedItemData);

        if(purchasedItems == null)
        {
            purchasedItems = "없음";
        }

        string equippedItems = GetItemList(equippedItemData);

        if(equippedItems == null)
        {
            equippedItems = "없음";
        }

        Text_PurchasedItemData.text = purchasedItems;
        Text_EquippedItemData.text = equippedItems;
    }

    private string GetItemList(List<string> itemData)
    {
        return string.Join(", ", itemData);
    }

    private void InvokeExitBUttonClicked()
    {
        OnExitButtonClicked?.Invoke();
    }
}
