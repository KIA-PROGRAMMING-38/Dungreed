using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IAttackable
{
    // IAattackable 기능 구현
    public abstract void Attack();

}
