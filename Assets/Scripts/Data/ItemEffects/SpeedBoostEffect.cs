using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/SpeedBoost")]
public class SpeedBoostEffect : ItemEffect
{
    public float boostRate;
    public override bool Use()
    {
        //속도증가 함수 받으면 작성
        return false;
    }
}
