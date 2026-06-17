using System;
using System.Collections.Generic;

[Serializable]
public class ItemData
{
    public string ItemId;
    public string ItemName;
    public int ScoreValue;
}

public interface IHittable
{
    void OnHit();
}

public interface IStealable
{
    List<ItemData> GetStealableItems();

    void OnItemStolen(ItemData item);
}

public enum CustomerState
{
    None,
    Idle,  // 가만히
    Walking,  // 걷기
    Reacting,  // 진상짓
    Detected,  // 도둑질 감지 
    Hit  // 박치기 피격
}

public enum CustomerType
{
    None,
    Normal, // 일반손님
    Jerk  // 진상손님
}

public enum CustomerRace  // 손님의 종? 종류인데 사용할지는 아직 계획에 없음
{
    None,
    Human,
    Animal,
    Alien
}