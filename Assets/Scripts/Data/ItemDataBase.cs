using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    UpgradeHealth, SpeedUp, MiniGamePointDouble
}

public abstract class ItemBase : ScriptableObject
{
    [SerializeField] public int Price;
    [SerializeField] public string Name;
    [SerializeField] public string Iconpath;

    public virtual bool Buy()
    {
        if (StoreManager.Instance._coins >= Price)
        {
            StoreManager.Instance.SpendCoins(Price);
            return true;
        }
        return false;
    }
}

public class ItemDataBase  // 소모템, 치장템은 아직 추가 안했음
{
    public static ItemDataBase Instance = new ItemDataBase();

    public List<PermanentItem> PermanentList = new List<PermanentItem>();

    public void LoadAllItems()  
    {
        PermanentItem[] loadedPermanentItems = Resources.LoadAll<PermanentItem>("StoreItemDatas/PermanentItem");

        foreach (PermanentItem item in loadedPermanentItems)
        {
            PermanentList.Add(item);
        }
        Debug.Log($"[ItemDatabase] Item 로드 완료, 총 {PermanentList.Count} 개 ");
    }
}



// 영구적인 효과 아이템 데이터
[CreateAssetMenu(fileName = "NewPermanentItem", menuName = "ScriptableObject/Permanent")]
public class PermanentItem : ItemBase
{
    public EffectType effectType;
}

// 치장아이템 데이터, 사용할지는 모르겠음
[CreateAssetMenu(fileName = "NewCosmeticItem", menuName = "ScriptableObject/Cosmetic")]
public class CosmeticItem : ItemBase
{

    // 치장 관련 변수 적기
}

// 소모품아이템 데이터, 사용할지는 모르겠음
[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "ScriptableObject/Consumable")]
public class ConsumableItem : ItemBase
{ 

    // 소모품 관련 변수 적기
}


