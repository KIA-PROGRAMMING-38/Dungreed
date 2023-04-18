using UnityEngine;

public enum DamageType
{
    Normal,
    Critical,
}

public struct DamageInfo
{
    public int Damage;
    public DamageType Type;
}

public interface IDamageable
{
    public void Hit(DamageInfo damage, GameObject sender);
}