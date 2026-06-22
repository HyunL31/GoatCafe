using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccessory : MonoBehaviour
{
    [SerializeField] private Transform Transform_Head;
    [SerializeField] private Transform Transform_Eye;
    [SerializeField] private PlayerAccessory OtherAccessory;

    // 추후 PlayerModel에서 가져올 예정
    [SerializeField] private List<CosmeticItem> PlayerItem = new List<CosmeticItem>();
    [SerializeField] private PlayerMoving PlayerMoving;

    private Dictionary<string, Transform> _slots;
    private Dictionary<string, GameObject> _usedSlot = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _existItems = new Dictionary<string, GameObject>();

    private void Awake()
    {
        _slots = new Dictionary<string, Transform> { { "Head", Transform_Head }, { "Eye", Transform_Eye} };
    }

    private void OnEnable()
    {
        InitItem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            ClearItem();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            UseItem(PlayerItem[0]).Forget();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            UseItem(PlayerItem[1]).Forget();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            UseItem(PlayerItem[2]).Forget();
        }
    }

    private void InitItem()
    {
        foreach (CosmeticItem item in PlayerMoving.EquippedItems)
        {
            ApplyItemVisual(item).Forget();
        }
    }

    private async UniTask UseItem(CosmeticItem item)
    {
        if (PlayerMoving.EquippedItems.Contains(item))
        {
            if (_usedSlot.TryGetValue(item.Slot, out GameObject obj))
            {
                obj.SetActive(false);
            }

            _usedSlot.Remove(item.Slot);
            PlayerMoving.EquippedItems.Remove(item);

            OtherAccessory?.RemoveItemVisual(item);
            return;
        }

        ClearSlot(item);
        await ApplyItemVisual(item);

        if (!PlayerMoving.EquippedItems.Contains(item))
        {
            PlayerMoving.EquippedItems.Add(item);
        }

        OtherAccessory?.ApplyItemVisual(item).Forget();
    }

    private async UniTask ApplyItemVisual(CosmeticItem item)
    {
        if (_existItems.TryGetValue(item.name, out GameObject existingItem))
        {
            existingItem.SetActive(true);
            _usedSlot[item.Slot] = existingItem;
        }
        else
        {
            GameObject prefab = await LoadUtil.Async.LoadPrefabAsync(item.PrefabPath);
            GameObject newItem = Instantiate(prefab, SetSlot(item));
            _usedSlot[item.Slot] = newItem;
            _existItems[item.name] = newItem;
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
        for (int i = PlayerMoving.EquippedItems.Count - 1; i >= 0; i--)
        {
            CosmeticItem equippedItem = PlayerMoving.EquippedItems[i];

            if (equippedItem.Slot == item.Slot)
            {
                if (_usedSlot.TryGetValue(equippedItem.Slot, out GameObject prevItem))
                {
                    prevItem.SetActive(false);
                }

                _usedSlot.Remove(equippedItem.Slot);
                PlayerMoving.EquippedItems.RemoveAt(i);

                OtherAccessory?.RemoveItemVisual(equippedItem);
            }
        }
    }

    private void ClearItem()
    {
        foreach(GameObject obj in _usedSlot.Values)
        {
            obj.SetActive(false);
        }

        PlayerMoving.EquippedItems.Clear();

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