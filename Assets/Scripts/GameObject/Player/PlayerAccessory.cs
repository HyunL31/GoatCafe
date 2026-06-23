using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerAccessory : MonoBehaviour
{
    [SerializeField] private Transform Transform_Head;
    [SerializeField] private Transform Transform_Eye;
    [SerializeField] private PlayerAccessory OtherAccessory;

    private Dictionary<string, Transform> _slots;
    private Dictionary<string, GameObject> _usedSlot = new Dictionary<string, GameObject>();

    private void Awake()
    {
        _slots = new Dictionary<string, Transform> { { "Head", Transform_Head }, { "Eye", Transform_Eye} };
    }

    private void OnEnable()
    {
        InitItem();
    }


    private void InitItem()
    {
        foreach (var item in ItemDataBase.Instance.CosmeticDic)
        {
            ApplyItemVisual(item.Value).Forget();
        }
    }

    public async UniTask UseItem(CosmeticItem item)
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
        await ApplyItemVisual(item);

        if (!StoreManager.Instance.IsEquipped(item))
        {
            StoreManager.Instance.AddEquipped(item);
        }

        OtherAccessory?.ApplyItemVisual(item).Forget();
    }

    private async UniTask ApplyItemVisual(CosmeticItem item)
    {
        if (StoreManager.Instance.TryGetItemPrefab(item.name, out GameObject existingItem))
        {
            existingItem.SetActive(true);
            _usedSlot[item.Slot] = existingItem;
        }
        else
        {
            GameObject prefab = await LoadUtil.Async.LoadPrefabAsync(item.PrefabPath);
            GameObject newItem = Instantiate(prefab, SetSlot(item));
            _usedSlot[item.Slot] = newItem;
            StoreManager.Instance.AddItemPrefab(item.name, newItem);
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
        return _slots.TryGetValue(item.Slot, out Transform transform) ? transform : null;
    }
}