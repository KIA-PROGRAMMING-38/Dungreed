using System.Collections;
using UnityEngine;

public class EnemyRanger : EnemyBase
{
    [SerializeField] private Transform _hand;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private LayerMask _projectileCollisionLayerMask;

    private BoxCollider2D _targetCollider;
    IEnumerator _shootCoroutine;


    protected override void Start()
    {
        base.Start();
        _shootCoroutine = Shoot();
    }

    protected override void Attack()
    {
    }

    public override void SetTarget()
    {
        base.SetTarget();
        _targetCollider = _target.GetComponent<BoxCollider2D>();    
        StartCoroutine(_shootCoroutine);
    }

    public IEnumerator Shoot()
    {
        while(true)
        {
            if(_target == null)
            {
                yield break;
            }

            _anim.SetTrigger(ID_EnemyTraceTrigger);
            SoundManager.Instance.EffectPlay("ArrowDraw", transform.position);
            float readyTime = 2f;

            while(readyTime > 0f)
            {
                readyTime -= Time.deltaTime;
                if(_target != null)
                {
                    Vector2 dir = (_targetCollider.bounds.center - _collider.bounds.center).normalized;
                    _hand.transform.right = dir;
                    Vector3 newScale = Vector3.one;
                    if(dir.x < 0f)
                    {
                        newScale.x = -1f;
                    }
                    else
                    {
                        newScale.x = 1f;
                    }
                    transform.localScale = newScale;
                }
                yield return null;
            }
            yield return YieldCache.WaitForSeconds(0.2f);
            _anim.SetTrigger(ID_EnemyAttackTrigger);
            SoundManager.Instance.EffectPlay(Data.AttackSoundName, transform.position);
            var projectile = GameManager.Instance.ProjectilePooler.GetProjectile();
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.Damage = _data.AttackDamage;
            projectile.InitProjectTile(_firePosition.position, _hand.transform.right, _data.ProjectileData, damageInfo);
            projectile.OwnerObject = gameObject;
            projectile.SetCollisionMask(_projectileCollisionLayerMask);
            yield return YieldCache.WaitForSeconds(1f);
            yield return null;
        }
    }

    protected override void Die()
    {
        base.Die();
        CreateDieFx();
        gameObject.SetActive(false);
    }
}
