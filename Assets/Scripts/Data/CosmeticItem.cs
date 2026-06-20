using UnityEngine;

[CreateAssetMenu(fileName = "NewCosmeticItem", menuName = "ScriptableObject/Cosmetic")]
public class CosmeticItem : ItemBase
{

    // 치장 관련 변수 적기
    public string PrefabPath;
    public string Slot;
}