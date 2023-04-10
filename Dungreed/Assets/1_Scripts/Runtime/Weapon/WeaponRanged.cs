using System;
using UnityEngine;


public abstract class WeaponRanged : WeaponBase
{
    [SerializeField] protected Transform _firePosition;
    [SerializeField] protected LayerMask _enemyLayerMask;
    public event Action<float> OnReload;

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
        if (_isReloading == true) return;

        if (_currentAmmoCount <= 0)
        {
            Reload();
            return;
        }

        _recoveryAim = true;
        float x = Mathf.Sign(_hand.transform.localScale.x);
        float angle = x == 1f ? Data.ReboundPower : -Data.ReboundPower;
        _hand.transform.rotation = _hand.transform.rotation * Quaternion.Euler(0, 0, angle);
        _reboundAimDirection = _hand.transform.right;

        _currentAmmoCount = Mathf.Max(0, _currentAmmoCount - 1);
        Fire();
        CameraEffect();
    }

    public override void Initialize()
    {
        base.Initialize();
        _currentAmmoCount = Data.MaxAmmoCount;
        _recoveryAimDuration = (1f / Data.AttackSpeedPerSecond);
        _reloadElapsedTime = Time.time;
        _reloadTime = Data.ReloadTime;
        OnReload += _hand.Owner.GetComponentAllCheck<ReloadBar>().Reload;
    }

    public override void WeaponHandle()
    {
        ReloadProcess();
        ReboundProcess();

        // R버튼 누르면 장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if(_currentAmmoCount == 0)
        {
            Reload();
        }
    }
    protected virtual void ReloadProcess()
    {
        if (_isReloading == true)
        {
            _reloadElapsedTime += Time.deltaTime;
            if (_reloadElapsedTime > _reloadTime)
            {
                _reloadElapsedTime = 0;
                _isReloading = false;
            }
        }
    }
    protected virtual void ReboundProcess()
    {
        if (_recoveryAim == true)
        {
            _recoveryAimElapsedTime += Time.deltaTime;
            Vector3 mouseVec = _hand.Owner.transform.position.MouseDir();

            _hand.transform.right = Utils.Math.Utility2D.EaseInBack(_reboundAimDirection, mouseVec, _recoveryAimElapsedTime / _recoveryAimDuration);

            if (_recoveryAimElapsedTime > _recoveryAimDuration)
            {
                _recoveryAimElapsedTime = 0f;
                _recoveryAim = false;
            }
        }
    }
    protected virtual void Fire()
    {
        // 발사체 생성 후 초기화
        var projectTile = GameManager.Instance.ProjectTilePooler.Get();
        int damage = UnityEngine.Random.Range(Data.MinDamage, Data.MaxDamage + 1);
        projectTile.InitProjectTile(_firePosition.position, _firePosition.position.MouseDir(), Data.Projectile, damage);
        projectTile.SetCollisionMask(_enemyLayerMask);
    }

    protected override void Reload() 
    {
        // Reload 애니메이션 용 이벤트
        if (_isReloading == true) return;

        _isReloading = true;
        OnReload?.Invoke(Data.ReloadTime);
        _currentAmmoCount = Data.MaxAmmoCount;
    }

}
