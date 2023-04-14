using System;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IAttackable
{

    [SerializeField] 
    protected WeaponData _data;
    protected WeaponHand _hand;

    public WeaponData Data { get { return _data; }}

    public virtual void Initialize() {}

    public void SetHand(WeaponHand hand) => _hand = hand;

    public abstract void Attack();

    public virtual void WeaponHandle() { }
    protected virtual void PlayCameraEffect(){ }

}
