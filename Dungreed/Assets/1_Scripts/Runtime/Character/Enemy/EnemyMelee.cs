using UnityEngine;

public class EnemyMelee : EnemyBase
{
    private bool _isAttacking;
    private bool _isTracing;
    private Vector2 _velocity;

    protected override void OnEnable()
    {
        base.OnEnable();
        _isAttacking = false;
        _isTracing = false;
        _velocity= Vector2.zero;
        _searchTrigger.EnterAction += SetTarget;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        _isAttacking = false;
        _isTracing = false;
    }

    protected void Update()
    {
        if (_target == null) return;

        _velocity = _rig2D.velocity;
        _velocity.x = 0f;

        Vector2 targetDir = _target.transform.position - transform.position;
        float dirX = Mathf.Sign(targetDir.x);
        float dirY = Mathf.Sign(targetDir.y);



        if(!_controller.IsJumping && !_controller.IsDownJumping && _isTracing)
        {
            Bounds bound = _collider.bounds;
            var hit = Physics2D.OverlapBox(bound.center, bound.size, 0f, _controller.EnemyMask);

            if (hit != null)
            {
                Attack();
                _anim.SetTrigger(ID_EnemyAttackTrigger);
                return;
            }
        }

        if (_isTracing && !_isAttacking)
        {
            if (dirY == 1f)
            {
                if (3f < Mathf.Abs(targetDir.y) && Mathf.Abs(targetDir.y) < 5f)
                {
                    _controller.Jump(ref _velocity);
                }
            }
            else if (dirY == -1f)
            {
                _controller.DownJump();
            }

            if (Mathf.Abs(targetDir.x) >= 0.2f)
                _controller.HorizontalMove(ref _velocity, dirX);
        }

        _rig2D.velocity = _velocity;

    }

    public override void SetTarget()
    {
        base.SetTarget();
        _isTracing = true;
        _anim.SetTrigger(ID_EnemyTraceTrigger);
    }

    protected override void Attack()
    {
        _velocity.x = 0f;
        _isTracing = false;
        _isAttacking = true;
    }

    // 애니메이션 이벤트 함수
    public void HitCheck()
    {
        SoundManager.Instance.EffectPlay(Data.AttackSoundName, transform.position);
        Bounds bound = _renderer.bounds;
        var hit = Physics2D.OverlapBox(bound.center, bound.size, 0f, _controller.EnemyMask);

        if (hit != null)
        {
            IDamageable obj = hit.GetComponent<IDamageable>();
            DamageInfo damageInfo = new DamageInfo() { Damage = _data.AttackDamage, Type = DamageType.Normal };
            obj?.Hit(damageInfo, gameObject);
        }
    }

    public void AttackEnd()
    {
        _isAttacking = false;
        _isTracing = true;
        _anim.SetTrigger(ID_EnemyTraceTrigger);
    }

    protected override void Die()
    {
        base.Die();
        CreateDieFx();
        gameObject.SetActive(false);
    }

}
