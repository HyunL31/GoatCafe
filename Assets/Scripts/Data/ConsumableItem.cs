using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "ScriptableObject/Consumable")]
public class ConsumableItem : ItemBase
{
    public ItemEffect effect;
    public KeyCode keyCode;

    public bool UseItem()
    {
        if (effect != null)
        {
            if (effect.Use()) return true;
            else return false;
        }
        else
        {
            Debug.LogError("[ConsumableItem] effect = null");
            return false;
        }
    }

}