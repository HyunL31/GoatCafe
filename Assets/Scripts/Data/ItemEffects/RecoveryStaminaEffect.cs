using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/RecoveryStamina")]
public class RecoveryStaminaEffect : ItemEffect
{
    public int recoveryAmount;
    public override void Use()
    {
        // 스태미나 채우는 함수 받으면 작성
    }
}
