using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "ScriptableObject/Consumable")]
public class ConsumableItem : ItemBase
{
    public ItemEffect effect;

    public void UseItem()
    {
        if (effect != null)
        {
            effect.Use();
        }
        else Debug.LogError("[ConsumableItem] effect = null");
    }
}