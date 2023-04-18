using System;
using UnityEngine;


public class WeaponRanged : WeaponBase
{
    [SerializeField] protected Transform _firePosition;
    [SerializeField] protected LayerMask _enemyLayerMask;

    protected bool _isReloading = false;
    protected float _reloadElapsedTime = 0f;
    protected float _reloadTime = 0f;

    protected int _currentAmmoCount;

    protected Vector2 _reboundAimDirection;
    protected bool _recoveryAim = false;
    protected float _recoveryAimDuration;
    protected float _recoveryAimElapsedTime;

    public override void Attack()
    {
        if (_isReloading == true || _recoveryAim == true) return;

        if (_currentAmmoCount <= 0)
        {
            Reload();
            return;
        }

        _currentAmmoCount = Mathf.Max(0, _currentAmmoCount - 1);

        Fire();

        _recoveryAim = true;
        float x = Mathf.Sign(_hand.transform.localScale.x);
        float angle = x == 1f ? Data.ReboundPower : -Data.ReboundPower;
        _hand.transform.rotation = _hand.transform.rotation * Quaternion.Euler(0, 0, angle);
        _reboundAimDirection = _hand.transform.right;

        PlayCameraEffect();
    }

    public override void Initialize()
    {
        base.Initialize();
        _currentAmmoCount = _data.MaxAmmoCount;
        _recoveryAimDuration = (1f / _data.AttackSpeedPerSecond);
        _reloadElapsedTime = Time.time;
        _reloadTime = _data.ReloadTime;
    }

    public override void WeaponHandle()
    {
        ReloadProcess();
        ReboundProcess();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (_currentAmmoCount == 0)
        {
            Reload();
        }
    }
    private void ReloadProcess()
    {
        if (_isReloading == true)
        {
            _reloadElapsedTime += Time.deltaTime;
            if (_reloadElapsedTime - 0.1f >= _reloadTime)
            {
                _reloadElapsedTime = 0;
                _isReloading = false;
            }
        }
    }
    private void ReboundProcess()
    {
        if (_recoveryAim == true)
        {
            _recoveryAimElapsedTime += Time.deltaTime;
            Vector3 mouseVec = _hand.transform.position.MouseDir();

            _hand.transform.right = Utils.Math.Utility2D.EaseInOutCirc(_reboundAimDirection, mouseVec, _recoveryAimElapsedTime / _recoveryAimDuration);
            if (_recoveryAimElapsedTime > _recoveryAimDuration)
            {
                _recoveryAimElapsedTime = 0f;
                _recoveryAim = false;
            }
        }
    }

    protected virtual void Fire()
    {
        var projectile = GameManager.Instance.ProjectilePooler.GetProjectile();
        DamageInfo damageInfo = new DamageInfo();
        int damage = UnityEngine.Random.Range(_data.MinDamage, _data.MaxDamage + 1);

        // 플레이어의 위력을 가져온다
        // 위력 수치 1마다 피해량 1% 증가
        int critChance = _hand.OwnerStatus.CriticalChance;
        int critDamage = _hand.OwnerStatus.CriticalDamage;
        int Power = _hand.OwnerStatus.Power;
        // 기본이 100

        int rand = UnityEngine.Random.Range(0, 101);

        if (rand < critChance)
        {
            damageInfo.Type = DamageType.Critical;
            damage = damage + (int)(damage * (critDamage / 100f));
        }

        // 피해량 증가
        int totalDamage = damage + (int)(damage * (Power / 100f));
        damageInfo.Damage = totalDamage;
        projectile.InitProjectTile(_firePosition.position, transform.right, _data.Projectile, damageInfo);
        projectile.SetCollisionMask(_enemyLayerMask);
    }

    private void Reload() 
    {
        if (_isReloading == true) return;

        _isReloading = true;
        _hand?.Reload(_reloadTime);
        _currentAmmoCount = _data.MaxAmmoCount;
    }

}
