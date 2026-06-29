using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/CustomerPeace")]
public class CustomerPeaceEffect : ItemEffect
{
    public float duration;
    public override bool Use()
    {
        if(StoreManager.Instance.IsPeaceItemUsed())
        {
            return false;
        }
        else
        {
            StoreManager.Instance.TriggerPeaceItem(duration);
            return true;
        }
    }
}
