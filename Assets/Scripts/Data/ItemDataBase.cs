using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    SpeedUp, MiniGamePointDouble, PointDouble, BonusDayDuration, MiniGameEasier, UnlockEmote
}

public enum CosmeticType
{
    Crown
}

public abstract class ItemBase : ScriptableObject
{
    [SerializeField] public int Price;
    [SerializeField] public string Name;
    [SerializeField] public string Iconpath;

    public virtual bool Buy()
    {
        StoreManager store = StoreManager.Instance;

        if (store._coins >= Price)
        {
            store.SpendCoins(Price);
            return true;
        }
        else return false;
    }
}

public class ItemDataBase  // 소모템, 치장템은 아직 추가 안했음
{
    public static ItemDataBase Instance = new ItemDataBase();

    public List<PermanentItem> PermanentList = new List<PermanentItem>();
    public List<ConsumableItem> ConsumableList = new List<ConsumableItem>();
    public Dictionary<string, CosmeticItem> CosmeticDic = new Dictionary<string, CosmeticItem>();

    public void LoadAllItems()  
    {
        PermanentItem[] loadedPermanentItems = Resources.LoadAll<PermanentItem>("StoreItemDatas/PermanentItem");

        foreach (PermanentItem item in loadedPermanentItems)
        {
            PermanentList.Add(item);
        }

        ConsumableItem[] loadedConsumableItems = Resources.LoadAll<ConsumableItem>("StoreItemDatas/ConsumableItem");

        foreach (ConsumableItem item in loadedConsumableItems)
        {
            ConsumableList.Add(item);
        }

        CosmeticItem[] loadedCosmeticItems = Resources.LoadAll<CosmeticItem>("StoreItemDatas/CosmeticItem");

        foreach (CosmeticItem item in loadedCosmeticItems)
        {
            CosmeticDic.Add(item.Name, item);
        }

        Debug.Log($"[ItemDatabase] PermanentItem 로드 완료, 총 {PermanentList.Count} 개 ");
        Debug.Log($"[ItemDatabase] ConsumableItem 로드 완료, 총 {ConsumableList.Count} 개 ");
        Debug.Log($"[ItemDatabase] CosmeticItem 로드 완료, 총 {CosmeticDic.Count} 개 ");
    }
}