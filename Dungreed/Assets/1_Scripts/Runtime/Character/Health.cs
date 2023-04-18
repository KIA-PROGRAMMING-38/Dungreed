using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _maxHp;
    [ShowOnly, SerializeField]
    private int _currentHp;

    public int MaxHp { get { return _maxHp; } }
    public int CurrentHp { get { return _currentHp; } }

    public float    InvincibleTime;
    public bool     IsInvincible { get;  set; }

    [SerializeField] 
    private float _flickingTime;

    [SerializeField] 
    private Material _flickingMaterial;
    private Material _defaultMaterial;

    private Renderer _renderer;

    public event Action OnInvincible;
    public event Action OnHit;
    public event Action OnDie;
    public event Action<GameObject> OnDieWithSender;
    public event Action OnHeal;
    public event Action OnRevive;

    public event Action<int, int> OnHealthChanged;

    private IEnumerator _flickingCoroutine;
    private IEnumerator _invincibleCoroutine;


    private void Awake()
    {
        _renderer = this.GetComponentAllCheck<SpriteRenderer>();
        Debug.Assert(_renderer != null, $"Invalid Renderer : {name}/{nameof(Health)}:Component");

        _flickingMaterial = _flickingMaterial ?? ResourceCache.GetResource<Material>("Materials/DefaultHitMaterial");
        _defaultMaterial = _renderer.material;

        _flickingCoroutine = FlickingCoroutine();
        _invincibleCoroutine = InvincibleCoroutine();

        if(_currentHp == 0)
        {
            _currentHp = _maxHp;
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
        _currentHp = _maxHp = maxHp;
        OnHealthChanged?.Invoke(_currentHp, _maxHp);
    }


    public void Hit(int damage, GameObject sender)
    {
        if (IsInvincible) return;

        int calcHp = _currentHp - damage;

        if (calcHp <= 0)
        {
            OnDie?.Invoke();

            OnDieWithSender?.Invoke(sender);
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
        OnRevive?.Invoke(); 
        OnHealthChanged(_currentHp, _maxHp);
    }

    public void Heal(int heal)
    {
        Debug.Assert(heal >= 0);
        _currentHp    = Mathf.Min(_currentHp + heal, _maxHp);
        OnHeal?.Invoke();
        OnHealthChanged?.Invoke(_currentHp, _maxHp);
    }

    private void Invincible()
    {
        StartCoroutine(_invincibleCoroutine);
    }

    private void Flicking()
    {
        StartCoroutine(_flickingCoroutine);
    }

    IEnumerator FlickingCoroutine()
    {
        while(true)
        {
            _renderer.material = _flickingMaterial;

            yield return YieldCache.WaitForSeconds(_flickingTime);

            _renderer.material = _defaultMaterial;
            StopCoroutine(_flickingCoroutine);

            yield return null;
        }
    }

    IEnumerator InvincibleCoroutine()
    {
        while(true)
        {
            IsInvincible = true;
            OnInvincible?.Invoke();

            yield return YieldCache.WaitForSeconds(InvincibleTime);

            IsInvincible = false;
            StopCoroutine(_invincibleCoroutine);

            yield return null;
        }
    }
}
