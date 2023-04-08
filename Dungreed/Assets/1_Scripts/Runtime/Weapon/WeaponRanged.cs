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

    public override void Attack()
    {
        if (_isReloading == true) return;

        if (_currentAmmoCount <= 0)
        {
            Reload();
            return;
        }

        _currentAmmoCount = Mathf.Max(0, _currentAmmoCount - 1);
        Fire();
        OnAttack?.Invoke();
    }

    public override void Initialize()
    {
        base.Initialize();
        _currentAmmoCount = Data.MaxAmmoCount;
        _reloadElapsedTime = Time.time;
        _reloadTime = Data.ReloadTime;
        OnReload += _hand.Owner.GetComponentAllCheck<ReloadBar>().Reload;
    }

    public override void WeaponHandle()
    {
        if(_isReloading == true)
        {
            _reloadElapsedTime += Time.deltaTime;
            if(_reloadElapsedTime > _reloadTime)
            {
                _reloadElapsedTime = 0;
                _isReloading = false;
            }
        }

        // R버튼 누르면 장전
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if(_currentAmmoCount == 0)
        {
            Reload();
        }

        
    }
    protected virtual void Fire()
    {
        // 발사체 생성 후 초기화
        var projectTile = GameManager.Instance.ProjectTilePooler.Get();
        int damage = UnityEngine.Random.Range(Data.MinDamage, Data.MaxDamage + 1);
        projectTile.InitProjectTile(_firePosition.position, _firePosition.position.MouseDir(), Data.ProjectTile, damage);
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
