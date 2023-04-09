using System;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IAttackable
{

    protected       WeaponHand _hand;
    public          WeaponData Data;

    public virtual void Initialize() {}

    public void SetHand(WeaponHand hand) => _hand = hand;

    public abstract void Attack();

    protected virtual void Reload() { }
    public virtual void WeaponHandle() { }
    protected virtual void CameraEffect(){ }

}
