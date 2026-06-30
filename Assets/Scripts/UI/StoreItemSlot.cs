using Cysharp.Threading.Tasks;
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



    public async UniTaskVoid Setup(ItemBase itemData)
    {
        itemBaseData = itemData;
        _itemNameText.text = itemBaseData.Name;
        _itemPriceText.text = itemBaseData.Price.ToString();
        var tempSprite = await LoadUtil.Async.LoadSpriteAsync(itemBaseData.Iconpath);
        _itemImage.sprite = tempSprite;

        _button.onClick.AddListener(OnClickBuyBtn);
    }

    private void OnClickBuyBtn()
    {
        StoreManager.Instance.HandleButtonClick(itemBaseData, _button);
    }


}
