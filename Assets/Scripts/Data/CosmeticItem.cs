using UnityEngine;

[CreateAssetMenu(fileName = "NewCosmeticItem", menuName = "ScriptableObject/Cosmetic")]
public class CosmeticItem : ItemBase
{
    public string PrefabPath;
    public string Slot;
    public CosmeticType type;
}