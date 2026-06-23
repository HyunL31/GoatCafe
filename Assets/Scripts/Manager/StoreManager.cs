using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : BaseMonoManager<StoreManager>
{
    [Header("Prefab")]
    [SerializeField] private GameObject _storeItems;

    [Header("Store Popup")]
    [SerializeField] private GameObject _storePopup;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private Transform _contentParent;

    [Header("Main Player Accessory")]
    [SerializeField] private PlayerAccessory _playerAccessory;

    public int Coin { get; private set; } = 9999999;

    private HashSet<ItemBase> purchasedItems = new HashSet<ItemBase>();  // 구매한 영구적/치장 아이템 보관
    private HashSet<CosmeticItem> equippedItems = new HashSet<CosmeticItem>();  // 치장 아이템 보관
    private Dictionary<string, GameObject> _existItems = new Dictionary<string, GameObject>();  // 치장아이템 오브젝트 (Goat)
    private Dictionary<string, GameObject> _existHumanoidItems = new Dictionary<string, GameObject>();  // 치장아이템 오브젝트 (Humanoid)
    private Dictionary<GameObject, string> _existItemsReverse = new Dictionary<GameObject, string>();  // 치장 아이템 프리팹 역방향 딕셔너리
    private Dictionary<ItemBase, int> inventoryDic = new Dictionary<ItemBase, int>();
    private Dictionary<ItemBase, StoreItemSlot> StoreSlotDic = new Dictionary<ItemBase, StoreItemSlot>();

    private void Start()
    {
        Coin = SaveManager.Instance.CurrentPlayerModel.Coin;
    }

    public void AddItemObj(string name, GameObject prefab)
    {
        if(!_existItems.ContainsKey(name))
        {
            _existItems.Add(name, prefab);
            _existItemsReverse.Add(prefab, name);
        }
    }
    public bool TryGetItemObj(string name, out GameObject item)
    {
        return _existItems.TryGetValue(name, out item);
    }
    public string GetPrefabName(GameObject item)
    {
        if(_existItemsReverse.ContainsKey(item))
        {
            return _existItemsReverse[item];
        }
        return null;
    }
    public void AddHumanoidItemObj(string name, GameObject prefab)
    {
        if(!_existHumanoidItems.ContainsKey(name))
        {
            _existHumanoidItems.Add(name, prefab);
        }
    }
    public bool TryGetHumanoidItemObj(string name, out GameObject item)
    {
        return _existHumanoidItems.TryGetValue(name, out item);
    }
    public bool IsPurchased(ItemBase itemData) => purchasedItems.Contains(itemData);
    public void AddPurchased(ItemBase itemData) => purchasedItems.Add(itemData);
    public void RemovePurchased(ItemBase itemData) => purchasedItems.Remove(itemData);
    public bool IsEquipped(CosmeticItem itemData) => equippedItems.Contains(itemData);
    public void AddEquipped(CosmeticItem itemData) => equippedItems.Add(itemData);
    public void RemoveEquipped(CosmeticItem itemData) => equippedItems.Remove(itemData);
    public void ClearEquippedData() => equippedItems.Clear();


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

        StoreItemSlot tempslot;
        foreach (var item in ItemDataBase.Instance.PermanentList)
        {
            GameObject slotObj = Instantiate(_storeItems, _contentParent);

            tempslot = slotObj.GetComponent<StoreItemSlot>();
            tempslot.Setup(item);
            StoreSlotDic.Add(item, tempslot);
        }
        foreach (var item in ItemDataBase.Instance.ConsumableList)
        {
            GameObject slotObj = Instantiate(_storeItems, _contentParent);

            tempslot = slotObj.GetComponent<StoreItemSlot>();
            tempslot.Setup(item);
            StoreSlotDic.Add(item, tempslot);
        }
        foreach (var item in ItemDataBase.Instance.CosmeticDic)
        {
            GameObject slotObj = Instantiate(_storeItems, _contentParent);

            tempslot = slotObj.GetComponent<StoreItemSlot>();
            tempslot.Setup(item.Value);
            StoreSlotDic.Add(item.Value, tempslot);
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
                case EffectType.MiniGameEasier:
                    Debug.Log("MiniGameEasier 구매됨");
                    
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
            if (equippedItems.Contains(cosmeticData))
            {
                _playerAccessory.UseItem(cosmeticData);
                ChangeSlotButtonState(cosmeticData, true);
            }
            else
            {
                _playerAccessory.UseItem(cosmeticData);
                ChangeSlotButtonState(cosmeticData, false);
            }
        }
    }

    public void ChangeSlotButtonState(ItemBase item, bool isEquipped)
    {
        if(StoreSlotDic.TryGetValue(item, out StoreItemSlot slot))
        {
            Debug.Log($"{item} 찾았음");
            if(isEquipped)
            {
                slot._itemPriceText.text = "Equip";
            }
            else
            {
                slot._itemPriceText.text = "UnEquip";
            }
            return;
        }
        Debug.Log($"{item} 못찾았음");
    }

    public void LoadSaveItemData()
    {
        PlayerModel model = SaveManager.Instance.CurrentPlayerModel;

        purchasedItems.Clear();
        equippedItems.Clear();

        foreach (string itemName in model.PurchasedItemNames)
        {
            if (ItemDataBase.Instance.CosmeticDic.TryGetValue(itemName, out var cosmeticItem))
            {
                purchasedItems.Add(cosmeticItem);
            }
        }

        foreach (string itemName in model.EquippedItemNames)
        {
            if (ItemDataBase.Instance.CosmeticDic.TryGetValue(itemName, out var cosmeticItem))
            {
                equippedItems.Add(cosmeticItem);

                if (_playerAccessory != null)
                {
                    _playerAccessory.UseItem(cosmeticItem);
                }
            }
        }
    }


    ////// 아래는 임시로 만든 함수 or 변수 (다른곳에서 만들어지면 지울 예정)
    


    public void SpendCoins(int amount)
    {
        if(Coin >= amount)
        {
            Coin -= amount;
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
        _coinText.text = Coin.ToString();
    }
}
