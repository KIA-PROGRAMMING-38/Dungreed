using System;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IAttackable
{

    protected       WeaponHand _hand;
    public          WeaponData Data;
    public          Action OnAttack;

    public void SetHand(WeaponHand hand) => _hand = hand;

    public virtual void EnemyHitCheck() { }
    public abstract void Attack();

}
