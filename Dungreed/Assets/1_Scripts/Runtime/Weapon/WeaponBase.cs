using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IAttackable
{
    // IAattackable 기능 구현

    protected WeaponHand _hand;

    public WeaponData Data;

    public void SetHand(WeaponHand hand) => _hand = hand;

    public abstract void Attack();

}
