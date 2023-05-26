using Cysharp.Threading.Tasks;
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

    public float InvincibleTime;
    public bool IsInvincible { get; set; }

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


    private void Awake()
    {
        _renderer = this.GetComponentAllCheck<SpriteRenderer>();
        Debug.Assert(_renderer != null, $"Invalid Renderer : {name}/{nameof(Health)}:Component");

        _flickingMaterial = _flickingMaterial ?? ResourceCache.GetResource<Material>("Materials/DefaultHitMaterial");
        _defaultMaterial = _renderer.material;

        if (_currentHp == 0)
        {
            _currentHp = _maxHp;
        }
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(_currentHp, _maxHp);
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


    public bool Hit(DamageInfo damageInfo, GameObject sender)
    {
        if (IsInvincible) return false;
        if (_currentHp <= 0) return false;

        Vector2 damageTextPos = transform.position;
        damageTextPos.y = _renderer.bounds.max.y;

        // Health를 가지고 있는 오브젝트의 레이어가 Enemy이면 데미지 팝업 나타나게
        if (Globals.LayerMask.CompareMask(gameObject.layer, Globals.LayerMask.Enemy))
        {
            GameManager.Instance.DamageTextPooler.PopupDamage(damageInfo, damageTextPos);
        }

        int calcHp = _currentHp - damageInfo.Damage;

        if (calcHp <= 0)
        {
            OnDie?.Invoke();

            OnDieWithSender?.Invoke(sender);
            calcHp = 0;
        }

        _currentHp = calcHp;

        OnHit?.Invoke();
        OnHealthChanged?.Invoke(_currentHp, _maxHp);

        return true;
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
        _currentHp = Mathf.Min(_currentHp + heal, _maxHp);
        OnHeal?.Invoke();
        OnHealthChanged?.Invoke(_currentHp, _maxHp);
    }

    private void Invincible()
    {
        InvincibleTask().Forget();
    }

    private void Flicking()
    {
        FlickingTask().Forget();
    }

    async UniTaskVoid FlickingTask()
    {
            _renderer.material = _flickingMaterial;

            await UniTask.Delay((int)(1000 * _flickingTime));

            _renderer.material = _defaultMaterial;

    }

    async UniTaskVoid InvincibleTask()
    {
        IsInvincible = true;
        OnInvincible?.Invoke();

        await UniTask.Delay((int)(1000 * InvincibleTime));

        IsInvincible = false;
    }
}
