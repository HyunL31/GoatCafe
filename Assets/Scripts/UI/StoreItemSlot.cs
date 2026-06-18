using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemSlot : MonoBehaviour
{
    private ItemBase _itemdata;
    [SerializeField] private Button _button;
    [SerializeField] private Image _Image;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemPriceText;

    public void Setup(ItemBase itemData)
    {
        _itemdata = itemData;
        _itemNameText.text = _itemdata.Name;
        _itemPriceText.text = _itemdata.Price.ToString();
       
        // _Image.sprite = Resources.Load<Sprite>(_itemdata.Iconpath);

        _button.onClick.AddListener(OnClickBuyBtn);
    }

    private void OnClickBuyBtn()
    {
        StoreManager.Instance.HandlePurchase(_itemdata, _button);
    }
}
