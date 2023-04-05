using UnityEngine;
using EnumTypes;
[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Prefab")]
    public GameObject   Prefab;

    public GameObject   ProjectTile;
    public GameObject   AttackEffect;
    [Space(10)]
    [Header("Weapon Datas")]
    public string Name;
    public string Description;

    public float    RotateAngleOffset;
    public float    SpriteDefaultRotateAngle;
    public Vector2  OffsetInitPosition;

    public WeaponAttackType AttackType;
    public WeaponEquipType  EquipType;
    public WeaponRarity     Rarity;

    public int MinDamage;
    public int MaxDamage;
    public float AttackSpeedPerSecond;

}
