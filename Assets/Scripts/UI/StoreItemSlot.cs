using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemSlot : MonoBehaviour
{
    private ItemBase _itemdata;
    [SerializeField] public Button _button;
    [SerializeField] public Image _itemImage;
    [SerializeField] public TextMeshProUGUI _itemNameText;
    [SerializeField] public TextMeshProUGUI _itemPriceText;



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
        StoreManager.Instance.HandleButtonClick(_itemdata, _button);
    }


}
