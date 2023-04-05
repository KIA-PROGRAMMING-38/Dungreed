using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{

    [SerializeField]
    private int _maxHp;
    private int _currentHp;
    private bool _initialized;
    public float InvincibleTime;

    [SerializeField] private float _flickingTime;
    [SerializeField] private Color _flickingColor;

    [SerializeField] 
    private Material _flickingMaterial;
    private Material _defaultMaterial;

    private Renderer _renderer;
    public bool IsInvincible { get; private set; }

    public event Action OnInvincible;
    public event Action OnHit;
    public event Action OnDie;

    public UnityEvent<float> OnHealthChanged;

    private void Awake()
    {
        _renderer = this.GetComponentAllCheck<SpriteRenderer>();
        if (_renderer == null)
            Debug.Log("rendere is null");
        Debug.Assert(_renderer != null, $"Invalid Renderer : {name}/{nameof(Health)}:Component");

        _defaultMaterial = _renderer.material;

    }

    private void Start()
    {
        // Start전에 초기화함수를 호출해주지 않았으면 Inspector에서 설정한 MaxHp로 초기화
        if (_initialized == false)
        {
            Initialize(_maxHp);
        }
    }

    private void OnEnable()
    {
        OnHit -= Invincible;
        OnHit -= Flicking;
        OnHit += Invincible;
        OnHit += Flicking;
    }
    private void OnDisable()
    {
        OnHit -= Invincible;
        OnHit -= Flicking;
    }

    public void Initialize(int maxHp)
    {
        _initialized = true;
        _currentHp = _maxHp = maxHp;
        float ratio = _currentHp / (float)_maxHp;
        OnHealthChanged?.Invoke(ratio);
    }
    private void Die()
    {
        OnDie?.Invoke();
    }

    public void Hit(int damage, GameObject hitter = null)
    {
        if (IsInvincible) return;

        int calcHp = _currentHp - damage;

        if (calcHp <= 0)
        {
            Die();
            calcHp = 0;
        }

        _currentHp = calcHp;
        float hpRatio = _currentHp / (float)_maxHp;

        OnHit?.Invoke();
        OnHealthChanged?.Invoke(hpRatio);
    }

    private void Invincible()
    {
        StartCoroutine(InvincibleCoroutine());
    }
    private void Flicking()
    {
        StartCoroutine(FlickingCoroutine());
    }

    IEnumerator FlickingCoroutine()
    {
        _renderer.material = _flickingMaterial;
        yield return YieldCache.WaitForSeconds(_flickingTime);
        _renderer.material = _defaultMaterial;
    }

    IEnumerator InvincibleCoroutine()
    {
        IsInvincible = true;

        OnInvincible?.Invoke();

        yield return YieldCache.WaitForSeconds(InvincibleTime);

        IsInvincible = false;
        Debug.Log(IsInvincible);
    }
}
