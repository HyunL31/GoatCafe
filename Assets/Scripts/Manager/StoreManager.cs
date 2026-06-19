using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : BaseMonoManager<StoreManager>
{

    //임시 소유 코인
    public int _coins = 999999;

    [Header("Prefab")]
    [SerializeField] private GameObject _storeItems;

    [Header("Store Popup")]
    [SerializeField] private GameObject _storePopup;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private Transform _contentParent;

    [Header("Cosmetic Object")]
    [SerializeField] private GameObject _crown;
    [SerializeField] private GameObject _humanoidCrown;

    private HashSet<ItemBase> purchasedItems = new HashSet<ItemBase>();  // 구매한 영구적/치장 아이템 보관
    private HashSet<ItemBase> equippedItems = new HashSet<ItemBase>();  // 치장 아이템 보관

    public bool IsPurchased(ItemBase itemData) => purchasedItems.Contains(itemData);
    public void AddPurchased(ItemBase itemData)
    {
        if (!purchasedItems.Contains(itemData))
        {
            purchasedItems.Add(itemData);
        }
    }
    public void RemovePurchased(ItemBase itemData)
    {
        if(purchasedItems.Contains(itemData))
        {
            purchasedItems.Remove(itemData);
        }
    }
    public bool IsEquipped(ItemBase itemData) => equippedItems.Contains(itemData);
    public void AddEquipped(ItemBase itemData)
    {
        if (!equippedItems.Contains(itemData))
        {
            equippedItems.Add(itemData);
        }
    }
    public void RemoveEquipped(ItemBase itemData)
    {
        if (equippedItems.Contains(itemData))
        {
            equippedItems.Remove(itemData);
        }
    }
    private Dictionary<ItemBase, int> inventoryDic = new Dictionary<ItemBase, int>();
    public void AddInventory(ItemBase itemData)
    {
        if(inventoryDic.ContainsKey(itemData))
        {
            inventoryDic[itemData] += 1;
        }
        else
        {
            inventoryDic.Add(itemData, 1);
        }
    }

    public void UseInventoryItem(ItemBase itemData)
    {
        if(inventoryDic.ContainsKey(itemData))
        {
            inventoryDic[itemData] -= 1;
        }
        if (inventoryDic[itemData] <= 0)
        {
            inventoryDic.Remove(itemData);
        }
    }

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnClickExitBtn);
        ItemDataBase.Instance.LoadAllItems();
        InitStorePopup();
    }

    private void InitStorePopup()
    {
        foreach (Transform child in _contentParent)
            Destroy(child.gameObject);

        foreach (var item in ItemDataBase.Instance.PermanentList)
        {
            GameObject slotObj = Instantiate(_storeItems, _contentParent);

            slotObj.GetComponent<StoreItemSlot>().Setup(item);
        }
        foreach (var item in ItemDataBase.Instance.ConsumableList)
        {
            GameObject slotObj = Instantiate(_storeItems, _contentParent);

            slotObj.GetComponent<StoreItemSlot>().Setup(item);
        }
        foreach (var item in ItemDataBase.Instance.CosmeticList)
        {
            GameObject slotObj = Instantiate(_storeItems, _contentParent);

            slotObj.GetComponent<StoreItemSlot>().Setup(item);
        }
    }

    public void OnClickExitBtn()
    {
        _storePopup.SetActive(false);

        SetCursorState(false);
    }

    public void OpenStorePopup()
    {
        UpdateStorePopup();

        _storePopup.SetActive(true);

        SetCursorState(true);
    }

    public void HandleButtonClick(ItemBase itemData, Button button)
    {
        if (!purchasedItems.Contains(itemData)) 
        {
            if (!itemData.Buy())
            {
                // 돈 부족하다는 메시지 띄우기
                Debug.Log("[StoreManager] Coin 부족");
                return;
            }
            else
            {
                AddPurchased(itemData);
            }
        }
        UpdateStorePopup();

        if (itemData is PermanentItem permanentData)  // 일단은 영구적인 효과의 아이템만 구현
        {
            button.interactable = false;

            switch (permanentData.effectType)   // 영구적 아이템 효과 적용 부분
            {
                case EffectType.UpgradeHealth:
                    Debug.Log("UpgradeHealth 구매됨");
                    break;
                case EffectType.SpeedUp:
                    Debug.Log("SpeedUp 구매됨");
                    break;
                case EffectType.MiniGamePointDouble:
                    Debug.Log("MiniGamePointDouble 구매됨");
                    break;
                case EffectType.BonusDayDuration:
                    Debug.Log("BonusDayDuration 구매됨");
                    GameManager.Instance.BonusDayDurationItemPurchased(permanentData.value);
                    break;
                case EffectType.PointDouble:
                    Debug.Log("PointDouble 구매됨");
                    GameManager.Instance.PointDoubleItemPurchased();
                    break;
            }
        }


        if(itemData is ConsumableItem consumableData)
        {
            if(inventoryDic.ContainsKey(consumableData))
            {
                inventoryDic[consumableData] += 1;
            }
            else
            {
                inventoryDic.Add(consumableData, 1);
            }
        }

        if(itemData is CosmeticItem cosmeticData)
        {
            button.transform.Find("Store_GoldIcon").gameObject.SetActive(false);
            if (equippedItems.Contains(itemData))
            {
                equippedItems.Remove(itemData);
                SetCosmetic(cosmeticData.type, false);
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            }
            else
            {
                equippedItems.Add(itemData);
                SetCosmetic(cosmeticData.type, true);
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Unequip";
            }

        }
    }

    private void SetCosmetic(CosmeticType type, bool isActive)
    {
        switch(type)
        {
            case CosmeticType.Crown:
                _crown.SetActive(isActive);
                _humanoidCrown.SetActive(isActive);
                break;
        }
    }


    ////// 아래는 임시로 만든 함수 or 변수 (다른곳에서 만들어지면 지울 예정)
    


    public void SpendCoins(int amount)
    {
        if(_coins >= amount)
        {
            _coins -= amount;
        }
    }

    //임시 마우스커서 활성화/비활성화 함수
    public void SetCursorState(bool state)
    {
        Cursor.visible = state;

        if (state) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    } 

    //임시 팝업 UI 업데이트
    public void UpdateStorePopup()
    {
        _coinText.text = _coins.ToString();
    }
}
