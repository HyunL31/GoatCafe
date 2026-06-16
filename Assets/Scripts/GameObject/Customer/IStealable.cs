using System.Collections.Generic;

public interface IStealable
{
    List<ItemData> GetStealableItems();

    void OnItemStolen(ItemData item);
}