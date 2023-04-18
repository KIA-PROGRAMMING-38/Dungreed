using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] 
    protected EnemyData _data;
    public EnemyData Data { get { return _data; } }

    protected EnemyController _controller;
    protected Health _health;
    protected Animator _anim;
    protected SpriteRenderer _renderer;
    protected Rigidbody2D _rig2D;
    protected BoxCollider2D _collider;
    protected Trigger _searchTrigger;

    public event Action OnDie;

    protected static readonly string EnemyDieFxPath = "EnemyDieFx";
    protected static readonly string EnemySpawnFxPath = "EnemySpawnFx";
    protected static readonly int ID_EnemyAttackTrigger = Animator.StringToHash("Attack");
    protected static readonly int ID_EnemyTraceTrigger = Animator.StringToHash("Trace");
    protected static readonly int ID_EnemyResetTrigger = Animator.StringToHash("Reset");

    [ShowOnly, SerializeField] protected GameObject _target;

    protected virtual void Awake()
    {
        _health = GetComponentInChildren<Health>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _anim = GetComponentInChildren<Animator>();
        _controller = GetComponentInChildren<EnemyController>();
        _searchTrigger = GetComponentInChildren<Trigger>();

        // 체력 세팅
        _health.Initialize(_data.MaxHp);
    }

    protected virtual void OnEnable()
    {
        _health.Initialize(_data.MaxHp);
        _target = null;
        _searchTrigger.OnTrigger();

        _health.OnDie -= Die;
        _health.OnDie += Die;
    }

    protected virtual void OnDisable()
    {
        _health.OnDie -= Die;
    }

    protected virtual void Start()
    {
        _rig2D = _controller.Rig2D;
        _collider = _controller.Collider;
    }

    public virtual void SetTarget()
    {
        _target = _searchTrigger.Collision.gameObject;
    }

    public virtual void ReleaseTarget()
    {
        _target = null;
        _searchTrigger.Collider.enabled = false;
        _anim.SetTrigger(ID_EnemyResetTrigger);
    }


    protected abstract void Attack();

    public void CreateSpawnFx()
    {
        Vector2 spawnPos = _collider.bounds.center;
        GameManager.Instance.FxPooler.GetFx(EnemySpawnFxPath, spawnPos, Quaternion.identity);
    }

    public void CreateDieFx()
    {
        Vector2 spawnPos = _collider.bounds.center;
        GameManager.Instance.FxPooler.GetFx(EnemyDieFxPath, spawnPos, Quaternion.identity);
    }

    protected virtual void Die()
    {
        OnDie?.Invoke();
    }
}
