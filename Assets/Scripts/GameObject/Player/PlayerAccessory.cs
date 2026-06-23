using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccessory : MonoBehaviour
{
    [SerializeField] private Transform Transform_Head;
    [SerializeField] private Transform Transform_Eye;
    [SerializeField] private PlayerAccessory OtherAccessory;

    private Dictionary<string, Transform> _slots;
    private Dictionary<string, GameObject> _usedSlot = new Dictionary<string, GameObject>();

    private Dictionary<string, Transform> Slots
    {
        get
        {
            if (_slots == null)
            {
                _slots = new Dictionary<string, Transform> { { "Head", Transform_Head }, { "Eye", Transform_Eye } };
            }
            return _slots;
        }
    }

    private void Start()
    {
        if (OtherAccessory != null)
        {
            InitItem();
            OtherAccessory.InitItem();
        }
    }


    private void InitItem()
    {
        foreach (var item in ItemDataBase.Instance.CosmeticDic)
        {
            SettingPrefabs(item.Value).Forget();
        }
    }

    public void UseItem(CosmeticItem item)
    {
        if (StoreManager.Instance.IsEquipped(item))
        {
            if (_usedSlot.TryGetValue(item.Slot, out GameObject obj))
            {
                obj.SetActive(false);
            }

            _usedSlot.Remove(item.Slot);
            StoreManager.Instance.RemoveEquipped(item);

            OtherAccessory?.RemoveItemVisual(item);
            return;
        }

        ClearSlot(item);
        ApplyItemVisual(item);

        if (!StoreManager.Instance.IsEquipped(item))
        {
            StoreManager.Instance.AddEquipped(item);
        }

        OtherAccessory?.ApplyItemVisual(item);
    }

    private async UniTask SettingPrefabs(CosmeticItem item)  // prefab 세팅용 초기화 함수
    {
        GameObject prefab = await LoadUtil.Async.LoadPrefabAsync(item.PrefabPath);
        GameObject newItem = Instantiate(prefab, SetSlot(item));
        newItem.SetActive(false);

        if (OtherAccessory == null)
        {
            StoreManager.Instance.AddHumanoidItemObj(item.name, newItem);
        }
        else
        {
            StoreManager.Instance.AddItemObj(item.name, newItem);
        }
    }

    private void ApplyItemVisual(CosmeticItem item)
    {
        GameObject existingItem;
        if (OtherAccessory != null)
        {
            StoreManager.Instance.TryGetItemObj(item.name, out existingItem);
        }
        else
        {
            StoreManager.Instance.TryGetHumanoidItemObj(item.name, out existingItem);
        }

        if (existingItem != null)
        {
            existingItem.SetActive(true);
            _usedSlot[item.Slot] = existingItem;
        }
    }

    public void RemoveItemVisual(CosmeticItem item)
    {
        if (_usedSlot.TryGetValue(item.Slot, out GameObject obj))
        {
            obj.SetActive(false);
        }

        _usedSlot.Remove(item.Slot);
    }

    private void ClearSlot(CosmeticItem item)
    {
        if (_usedSlot.TryGetValue(item.Slot, out GameObject prevItem))
        {
            prevItem.SetActive(false);
            string itemName = StoreManager.Instance.GetPrefabName(prevItem);
            if(ItemDataBase.Instance.CosmeticDic.TryGetValue(itemName, out CosmeticItem value))
            {
                StoreManager.Instance.ChangeSlotButtonState(value, true);
                StoreManager.Instance.RemoveEquipped(value);
            }
        }

        _usedSlot.Remove(item.Slot);

        OtherAccessory?.RemoveItemVisual(item);
    }

    private void ClearItem()
    {
        foreach(GameObject obj in _usedSlot.Values)
        {
            obj.SetActive(false);
        }

        StoreManager.Instance.ClearEquippedData();

        OtherAccessory?.ClearVisualOnly();
    }

    public void ClearVisualOnly()
    {
        foreach (GameObject obj in _usedSlot.Values)
        {
            obj.SetActive(false);
        }
        _usedSlot.Clear();
    }

    private Transform SetSlot(CosmeticItem item)
    {
        return Slots.TryGetValue(item.Slot, out Transform transform) ? transform : null;
    }
}