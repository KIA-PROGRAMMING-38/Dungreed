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

    protected static readonly string EnemyDieFxPath = "EnemyDieFx";
    protected static readonly string EnemySpawnFxPath = "EnemySpawnFx";
    protected static readonly int ID_EnemyAttackTrigger = Animator.StringToHash("Attack");
    protected static readonly int ID_EnemyTraceTrigger = Animator.StringToHash("Trace");

    [ShowOnly, SerializeField] protected GameObject _target;
    [SerializeField] protected ParticleSystem _dieParticle;

    protected virtual void Awake()
    {
        _health = GetComponent<Health>();
        _renderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _controller = GetComponent<EnemyController>();
        _searchTrigger = GetComponentInChildren<Trigger>();

        // 체력 세팅
        _health.Initialize(_data.MaxHp);
    }

    protected virtual void OnEnable()
    {
        _health.Initialize(_data.MaxHp);
        _target = null;
        _searchTrigger.OnTrigger();
    }

    protected virtual void OnDisable()
    {

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
}
