using System;
using UnityEngine;


public class WeaponRanged : WeaponBase
{
    [SerializeField] protected Transform _firePosition;
    [SerializeField] protected LayerMask _enemyLayerMask;

    protected int _currentAmmoCount;
    public int CurrentAmmoCount { get { return _currentAmmoCount; } }
    protected bool _isReloading = false;
    protected float _reloadElapsedTime = 0f;
    protected float _reloadTime = 0f;

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
        SoundManager.Instance.EffectPlay(Data.FireSoundName, transform.position);

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
        _reloadElapsedTime = 0;
        _reloadTime = _data.ReloadTime;

        _hand.OnWeaponChanged -= OnWeaponChanged;
        _hand.OnWeaponChanged += OnWeaponChanged;
    }

    protected virtual void OnDisable()
    {
        if (_hand != null)
        {
            _hand.OnWeaponChanged -= OnWeaponChanged;
        }
    }

    public void OnWeaponChanged()
    {
        _isReloading = false;
        _reloadElapsedTime = 0f;
        _currentAmmoCount = _data.MaxAmmoCount;
        _recoveryAim = false;
        _recoveryAimElapsedTime = 0f;
    }

    public override void WeaponHandle()
    {
        ReloadProcess();
        ReboundProcess();

        if (Input.GetKeyDown(KeyCode.R) && _currentAmmoCount != _data.MaxAmmoCount)
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
            float ratio = Mathf.Clamp01(_reloadElapsedTime / _reloadTime);
            if (_reloadElapsedTime >= _reloadTime)
            {
                _currentAmmoCount = _data.MaxAmmoCount;
                _reloadElapsedTime = 0f;
                _isReloading = false;
            }
            _hand?.Reload(ratio);
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
        SoundManager.Instance.EffectPlay(Data.ReloadSoundName, _hand.Owner.transform.position);
        _isReloading = true;
    }

}
