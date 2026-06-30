using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemSlot : MonoBehaviour
{
    public ItemBase itemBaseData;
    [SerializeField] public Button _button;
    [SerializeField] public Image _itemImage;
    [SerializeField] public Image _goldImage;
    [SerializeField] public TextMeshProUGUI _itemNameText;
    [SerializeField] public TextMeshProUGUI _itemPriceText;



    public void Setup(ItemBase itemData)
    {
        itemBaseData = itemData;
        _itemNameText.text = itemBaseData.Name;
        _itemPriceText.text = itemBaseData.Price.ToString();

        // _Image.sprite = Resources.Load<Sprite>(itemBaseData.Iconpath);

        _button.onClick.AddListener(OnClickBuyBtn);
    }

    private void OnClickBuyBtn()
    {
        StoreManager.Instance.HandleButtonClick(itemBaseData, _button);
    }


}
