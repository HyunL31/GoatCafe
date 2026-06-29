using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : BaseMonoManager<StoreManager>
{

    //임시 소유 코인
    public int Coin { get; set; }
    [Header("Customer Spawner")]
    [SerializeField] private CustomerSpawner _customerSpawner;

    [Header("Item Desc Tooltip")]  // UIManager로 옮기기 전 임시 구현 변수
    [SerializeField] private int DescTooltipWidth = 500;
    [SerializeField] private RectTransform UICanvasRect;
    [SerializeField] private GameObject ItemDescPopup;
    [SerializeField] public TextMeshProUGUI _itemDesctext;
    [SerializeField] public TextMeshProUGUI _itemNametext;

    [Header("Prefab")]
    [SerializeField] private GameObject _storeItems;

    [Header("Store Popup")]
    [SerializeField] private GameObject _storePopup;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private RectTransform _contentParent;
    [SerializeField] private ScrollRect _scrollRect;

    [Header("Main Player Accessory")]
    [SerializeField] private PlayerAccessory _playerAccessory;

    private HashSet<ItemBase> purchasedItems = new HashSet<ItemBase>();  // 구매한 영구적/치장 아이템 보관
    private HashSet<CosmeticItem> equippedItems = new HashSet<CosmeticItem>();  // 치장 아이템 보관

    private Dictionary<string, GameObject> _existItems = new Dictionary<string, GameObject>();  // 치장아이템 오브젝트 (Goat)
    private Dictionary<string, GameObject> _existHumanoidItems = new Dictionary<string, GameObject>();  // 치장아이템 오브젝트 (Humanoid)
    private Dictionary<GameObject, string> _existItemsReverse = new Dictionary<GameObject, string>();  // 치장 아이템 프리팹 역방향 딕셔너리

    private Dictionary<ItemBase, int> inventoryDic = new Dictionary<ItemBase, int>();
    private Dictionary<KeyCode, ItemBase> keySelectDic = new Dictionary<KeyCode, ItemBase>();

    private Dictionary<ItemBase, StoreItemSlot> StoreSlotDic = new Dictionary<ItemBase, StoreItemSlot>();

    public static event System.Action<PermanentItem> OnItemPurchased;
    public static event Action StoreAreaEntered;
    public static event Action<float> OnPeaceItemUsed;

    public void AddItemObj(string name, GameObject prefab)
    {
        if (!_existItems.ContainsKey(name))
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
        if (_existItemsReverse.ContainsKey(item))
        {
            return _existItemsReverse[item];
        }
        return null;
    }
    public void AddHumanoidItemObj(string name, GameObject prefab)
    {
        if (!_existHumanoidItems.ContainsKey(name))
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
        if (inventoryDic.ContainsKey(itemData))
        {
            inventoryDic[itemData] += 1;
        }
        else
        {
            inventoryDic.Add(itemData, 1);
        }
    }

    public bool UseInventoryItem(ItemBase itemData)
    {
        if (itemData is ConsumableItem consumable)
        {
            if (consumable.UseItem())
            {
                inventoryDic[itemData] -= 1;
                if (inventoryDic[itemData] <= 0)
                {
                    inventoryDic.Remove(itemData);
                }

                return true;
            }
        }

        return false;
    }

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnClickExitBtn);
        InputManager.Instance.OnItemUseBtnPressed += HandleItemUseButtonPressed;
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
            tempslot.Setup(item.Value);
            StoreSlotDic.Add(item.Value, tempslot);
        }
        foreach (var item in ItemDataBase.Instance.ConsumableList)
        {
            GameObject slotObj = Instantiate(_storeItems, _contentParent);

            tempslot = slotObj.GetComponent<StoreItemSlot>();
            tempslot.Setup(item.Value);
            StoreSlotDic.Add(item.Value, tempslot);
            keySelectDic.Add(item.Value.keyCode, item.Value);
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
        RectTransform popupRect = _storePopup.GetComponent<RectTransform>();
        GameManager.Instance.ResumeGame();
        UIEffectUtil.SetScaleZero(_storePopup.GetComponent<RectTransform>(), 0.3f);
        //UIEffectUtil.SetUISlideDown(popupRect, Vector2.zero, 0.5f);


    }

    public void OpenStorePopup()
    {
        Coin = SaveManager.Instance.CurrentPlayerModel.Coin;  // 테스트용 임시 코드인듯?? 나중에 수정하기
        UpdateStorePopup();
        _storePopup.SetActive(true);
        GameManager.Instance.PauseGame();
        UIEffectUtil.SetScaleOne(_storePopup.GetComponent<RectTransform>(), 0.3f);
        //UIEffectUtil.SetUISlideUp(_storePopup.GetComponent<RectTransform>(), 0.5f);

    }

    public void HandleButtonClick(ItemBase itemData, Button button)
    {
        Debug.Log($"{itemData.Name}, {button.name}");
        if (!purchasedItems.Contains(itemData))
        {
            if (!itemData.Buy())
            {
                NotificationManager.Instance.ShowNotification("Not Enough Coins!!!", Color.red);
                Debug.Log("[StoreManager] Coin 부족");
                return;
            }
            else
            {
                NotificationManager.Instance.ShowNotification($"{itemData.Name} Purchased", Color.green);
                if(!(itemData is ConsumableItem consumable))
                {
                    AddPurchased(itemData);
                }
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
                    GameManager.Instance.GoatSpeedBoostPurchased(permanentData.value);
                    break;
                case EffectType.MiniGamePointDouble:
                    Debug.Log("MiniGamePointDouble 구매됨");
                    MiniGameManager.Instance.SetMiniGameEasier(true);
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
                    MiniGameManager.Instance.SetMiniGameScoreDouble(true);
                    break;
                case EffectType.UnlockEmote:
                    Debug.Log("UnlockEmote 구매됨");
                    OnItemPurchased.Invoke(permanentData);
                    break;

            }
        }


        if (itemData is ConsumableItem consumableData)
        {
            AddInventory(itemData);
        }

        if (itemData is CosmeticItem cosmeticData)
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
        if (StoreSlotDic.TryGetValue(item, out StoreItemSlot slot))
        {
            Debug.Log($"{item} 찾았음");
            if (isEquipped)
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

    public void LoadSaveStore()
    {
        PlayerModel playerModel = SaveManager.Instance.CurrentPlayerModel;

        purchasedItems.Clear();
        equippedItems.Clear();

        foreach (string itemName in playerModel.PurchasedItemNames)
        {
            if (ItemDataBase.Instance.CosmeticDic.TryGetValue(itemName, out var cosmeticData))
            {
                purchasedItems.Add(cosmeticData);
            }
        }

        foreach (string itemName in playerModel.EquippedItemNames)
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

    public void SaveStoreData()
    {
        PlayerModel playerModel = SaveManager.Instance.CurrentPlayerModel;

        playerModel.PurchasedItemNames.Clear();
        playerModel.EquippedItemNames.Clear();

        foreach (var item in purchasedItems)
        {
            if (item != null)
            {
                playerModel.PurchasedItemNames.Add(item.Name);
            }
        }

        foreach (var item in equippedItems)
        {
            if (item != null)
            {
                playerModel.EquippedItemNames.Add(item.Name);
            }
        }

        playerModel.Coin = this.Coin;

        SaveManager.Instance.SaveData();
    }

    private void HandleItemUseButtonPressed(KeyCode code)
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;
        if (!keySelectDic.ContainsKey(code)) return;

        ItemBase curItem = keySelectDic[code];

        if (inventoryDic.ContainsKey(keySelectDic[code]))
        {
            if(UseInventoryItem(curItem))
            {
                NotificationManager.Instance.ShowNotification($"{curItem.Name} Used", Color.green);
            }
            else
            {
                NotificationManager.Instance.ShowNotification($"You Can't Use {curItem.Name}", Color.red);
            }
        }
        else
        {
            NotificationManager.Instance.ShowNotification($"You Don't Have {curItem.Name}!!!", Color.red);
        }
    }


    public bool IsPeaceItemUsed()
    {
        return _customerSpawner.IsPeaceItemUsed;
    }









    // 아이템 설명 팝업 관련 변수 / 함수들

    public void UpdateDescPopupPosition()
    {
        RectTransform popupTransform = ItemDescPopup.GetComponent<RectTransform>();
        Vector3 mousePos = Input.mousePosition;


        Vector3 targetPos = mousePos;

        if (targetPos.x + DescTooltipWidth > UICanvasRect.rect.width)
        {
            targetPos.x = mousePos.x - DescTooltipWidth;
        }

        popupTransform.position = targetPos;
    }
    public void SetitemDescPopup(bool isactive, ItemBase item)
    {
        if (isactive)
        {
            _itemDesctext.text = item.ItemDesc;
            _itemNametext.text = item.Name;
            ItemDescPopup.SetActive(isactive);
            LayoutRebuilder.ForceRebuildLayoutImmediate(ItemDescPopup.GetComponent<RectTransform>());
        }
        else
        {
            ItemDescPopup.SetActive(isactive);
        }
    }

    public void TriggerPeaceItem(float duration)
    {
        OnPeaceItemUsed.Invoke(duration);
    }


    ////// 아래는 임시로 만든 함수 or 변수 (다른곳에서 만들어지면 지울 예정)


    public void SpendCoins(int amount)
    {
        if (Coin >= amount)
        {
            Coin -= amount;
        }
    }

    //임시 팝업 UI 업데이트
    public void UpdateStorePopup()
    {
        _coinText.text = Coin.ToString();
    }

    public bool IsActiveStore()
    {
        return _storePopup.activeSelf;
    }
}
