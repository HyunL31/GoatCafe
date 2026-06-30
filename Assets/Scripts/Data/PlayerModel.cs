using System;
using System.Collections.Generic;

[Serializable]
public class InventorySaveData
{
    public string ItemName;
    public int Count;
}

[Serializable]
public class PlayerModel
{
    public int Coin;
    public int StolenItemCount;

    public int Day;
    public bool IsFirstDay;
    public string Ending;

    public int Stamina;
    public float WalkSpeed;
    public float RunSpeed;

    public List<string> PurchasedItemNames = new List<string>();
    public List<string> EquippedItemNames = new List<string>();
    public List<InventorySaveData> Inventory = new List<InventorySaveData>();
}