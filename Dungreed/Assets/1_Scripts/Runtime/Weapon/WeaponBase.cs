using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IAttackable
{
    // IAattackable ��� ����
    public abstract void Attack();

}
