using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyTurret : EnemyBase
{
    [SerializeField] private Transform _firePosition;
    [SerializeField] private LayerMask _projectileCollisionLayerMask;
    [SerializeField] private float _projectileSpawnAngleInterval;

    [SerializeField] private Bullet _bulletPrefab;
    ObjectPool<Bullet> _bulletPool;


    protected override void Awake()
    {
        base.Awake();
        _bulletPool = new ObjectPool<Bullet>(CreateFunc, GetAction, ReleaseAction, DestroyAction, false, 20, 300);
    }

    public void Fire()
    {
        Attack();
    }

    protected override void Attack()
    {
        Shoot().Forget();
    }

    public async UniTaskVoid Shoot()
    {
        while (true)
        {
            if (_target == null) break;
            _anim.SetTrigger(ID_EnemyAttackTrigger);
            await UniTask.Delay(5000);
        }
    }

    public void CreateProjectile()
    {
        _firePosition.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (_projectileSpawnAngleInterval == 0f) _projectileSpawnAngleInterval = 1f;
        SoundManager.Instance.EffectPlay(Data.AttackSoundName, transform.position);
        for (float i = 0f; i < 360f; i += _projectileSpawnAngleInterval)
        {
            var projectile = _bulletPool.Get();
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.Damage = _data.AttackDamage;
            _firePosition.transform.rotation = Quaternion.Euler(0, 0, i);
            projectile.InitBullet(transform.position, _firePosition.transform.right, damageInfo);
        }
    }

    protected override void Die()
    {
        base.Die();
        CreateDieFx();
        gameObject.SetActive(false);
    }

    #region Pool Functinon

    private Bullet CreateFunc()
    {
        Bullet v = Instantiate(_bulletPrefab, transform);
        v.SetOwner(_bulletPool);
        v.SetOwnerObject(gameObject);
        v.gameObject.SetActive(false);
        return v;
    }

    private void GetAction(Bullet projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void ReleaseAction(Bullet projectile)
    {
        projectile.ResetBullet();
        projectile.gameObject.SetActive(false);
    }

    private void DestroyAction(Bullet projectile)
    {
        projectile.ResetBullet();
        projectile.gameObject.SetActive(false);
    }

    #endregion
}
