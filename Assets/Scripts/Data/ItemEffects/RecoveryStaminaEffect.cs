using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/RecoveryStamina")]
public class RecoveryStaminaEffect : ItemEffect
{
    public int recoveryAmount;
    public override void Use()
    {
        GameManager.Instance.GoatStaminaItemUsed(recoveryAmount);
    }
}
