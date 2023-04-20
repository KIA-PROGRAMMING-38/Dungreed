using System;
using UnityEngine;

public class BelialLaser : MonoBehaviour
{
    [SerializeField]
    private BossBelialHand _ownerHand;
    private BoxCollider2D _collider;
    public bool IsAttackEnd { get; private set; }
    public event Action OnLaserFireEnd;

    private int _damagae;

    public void Initialize(int damage)
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = false;
        _collider.isTrigger = true;
        _damagae = damage;
        gameObject.SetActive(false);
    }

    public void Fire()
    {
        IsAttackEnd = false;
        gameObject.SetActive(true);
    }

    public void AttackStart()
    {
        _collider.enabled = true;
    }   

    public void HitCheckEnd()
    {
        _collider.enabled = false;
    }

    public void AttackEnd()
    {
        IsAttackEnd = true;
        gameObject.SetActive(false);
        OnLaserFireEnd?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Globals.LayerMask.CompareMask(collision.gameObject.layer, Globals.LayerMask.Player))
        {
            IDamageable damageable = collision.GetComponent<Health>();
            damageable.Hit(new DamageInfo() { Damage = _damagae }, _ownerHand.MainBody.gameObject);
        }
    }
}

