using System;
using System.Collections.Generic;

[Serializable]
public class PlayerModel
{
    public int Gold;

    public int Day;
    public bool IsFirstDay;

    public int Stamina;
    public float WalkSpeed;
    public float RunSpeed;

    HashSet<ItemBase> PurchasedItems = new HashSet<ItemBase>();
    HashSet<ItemBase> EquippedItems = new HashSet<ItemBase>();
}