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

        _button.interactable = true;
        _goldImage.ActiveTrue();

        _button.onClick.AddListener(OnClickBuyBtn);
    }

    public void Reset()
    {
        if (itemBaseData == null) return;

        _itemNameText.text = itemBaseData.Name;
        _itemPriceText.text = itemBaseData.Price.ToString();
        _button.interactable = true;
        _goldImage.ActiveTrue();
        Debug.Log($"[StoreItemSlot] {itemBaseData.Name} - goldimage {_goldImage.gameObject.activeSelf}");
    }

    private void OnClickBuyBtn()
    {
        StoreManager.Instance.HandleButtonClick(itemBaseData, _button);
    }


}
