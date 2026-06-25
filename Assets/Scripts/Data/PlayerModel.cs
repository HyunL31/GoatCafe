using System;
using System.Collections.Generic;

[Serializable]
public class PlayerModel
{
    public int Coin;
    public int StolenItemCount;

    public int Day;
    public bool IsFirstDay;

    public int Stamina;
    public float WalkSpeed;
    public float RunSpeed;

    public List<string> PurchasedItemNames = new List<string>();
    public List<string> EquippedItemNames = new List<string>();
}