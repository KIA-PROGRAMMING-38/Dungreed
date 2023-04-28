using UnityEngine;
using EnumTypes;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    public Sprite DefaultSprite;
    public ProjectileData   Projectile;

    public string SwingFxName;
    public string SwingSoundName;
    public string FireSoundName;
    public string ReloadSoundName;

    public int Id;
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

    public float MeleeAttackRange;

    // 탄창 최대 수
    public int   MaxAmmoCount;
    public float ReloadTime;
    public float ReboundPower;
}
