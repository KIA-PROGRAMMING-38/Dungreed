using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IAttackable
{
    // IAattackable ��� ����

    protected WeaponHand _hand;

    public WeaponData Data;

    public void SetHand(WeaponHand hand) => _hand = hand;

    public abstract void Attack();

}
