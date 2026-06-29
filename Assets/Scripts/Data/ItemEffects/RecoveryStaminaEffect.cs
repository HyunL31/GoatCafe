using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/RecoveryStamina")]
public class RecoveryStaminaEffect : ItemEffect
{
    public int recoveryAmount;
    public override bool Use()
    {
        GameManager.Instance.GoatStaminaItemUsed(recoveryAmount);
        return true;
    }
}
