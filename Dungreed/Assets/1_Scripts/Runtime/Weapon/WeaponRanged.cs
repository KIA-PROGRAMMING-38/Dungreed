using System;
using UnityEngine;


public class WeaponRanged : WeaponBase
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
            if (_reloadElapsedTime > _reloadTime)
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
        var projectTile = GameManager.Instance.ProjectTilePooler.Get();
        int damage = UnityEngine.Random.Range(Data.MinDamage, Data.MaxDamage + 1);
        projectTile.InitProjectTile(_firePosition.position, transform.right, Data.Projectile, damage);
        projectTile.SetCollisionMask(_enemyLayerMask);
    }

    private void Reload() 
    {
        // Reload �ִϸ��̼� �� �̺�Ʈ
        if (_isReloading == true) return;

        _isReloading = true;
        OnReload?.Invoke(Data.ReloadTime);
        _currentAmmoCount = Data.MaxAmmoCount;
    }

}
