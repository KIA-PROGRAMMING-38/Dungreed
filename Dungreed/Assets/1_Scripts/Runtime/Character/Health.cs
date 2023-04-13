using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{

    [SerializeField]
    private int _maxHp;
    [ShowOnly, SerializeField]
    private int _currentHp;

    public float InvincibleTime;

    public bool IsInvincible { get; private set; }
    public int MaxHp { get { return _maxHp; } }
    public int CurrentHp { get { return _currentHp; } }

    [SerializeField] private float _flickingTime;
    [SerializeField] 
    private Material _flickingMaterial;
    private Material _defaultMaterial;

    private Renderer _renderer;

    public event Action OnInvincible;
    public event Action OnHit;
    public event Action OnDie;

    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        _renderer = this.GetComponentAllCheck<SpriteRenderer>();
        Debug.Assert(_renderer != null, $"Invalid Renderer : {name}/{nameof(Health)}:Component");

        _flickingMaterial = _flickingMaterial ?? ResourceCache.GetResource<Material>("Materials/HitMaterial");
        _defaultMaterial = _renderer.material;
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
        _currentHp = _maxHp = maxHp;
        OnHealthChanged?.Invoke(_currentHp, _maxHp);
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Die");
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

        OnHit?.Invoke();
        OnHealthChanged?.Invoke(_currentHp, _maxHp);
    }

    public void Revive()
    {
        gameObject.SetActive(true);
        _currentHp = _maxHp;
        OnHealthChanged(_currentHp, _maxHp);
    }

    public void Heal(int heal)
    {
        Debug.Assert(heal >= 0);
        _currentHp    = Mathf.Min(_currentHp + heal, _maxHp);
        OnHealthChanged?.Invoke(_currentHp, _maxHp);
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
    }
}
