
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : BaseController
{
    private bool _isDie;
    public bool IsDie { get { return _isDie; } set { _isDie = value; } }
    public bool IsDashing { get; set; }
    public bool StopControll { get; private set; }
    public bool IsRevive { get; set; }
    #region Components
    private Health _health;
    private PlayerData _data;
    private PlayerInput _input;
    private Animator _animator;
    private SpriteRenderer _renderer;
    private WeaponHand _weaponHand;

    private PlayerHorizontalMovement _horizontalMovement;

    public Animator Anim { get { return _animator; } }
    public PlayerInput Input { get { return _input; } }
    public SpriteRenderer Renderer { get { return _renderer; } }
    public WeaponHand Hand { get { return _weaponHand; } }
    public PlayerHorizontalMovement HorizontalMovement { get { return _horizontalMovement; } }

    public event Action<int, int> OnDashAction;
    #endregion


    public readonly int Id_DieAnimationParameter = Animator.StringToHash(PlayerAnimParmaeterLiteral.DieTrigger);
    public readonly int Id_DashAnimationParameter = Animator.StringToHash(PlayerAnimParmaeterLiteral.DashTrigger);
    public readonly int Id_JumpAnimationParameter = Animator.StringToHash(PlayerAnimParmaeterLiteral.JumpTrigger);
    public readonly int Id_RunAnimationParameter = Animator.StringToHash(PlayerAnimParmaeterLiteral.RunTrigger);
    public readonly int Id_IdleAnimationParameter = Animator.StringToHash(PlayerAnimParmaeterLiteral.IdleTrigger);
    public readonly int Id_FallAnimationParameter = Animator.StringToHash(PlayerAnimParmaeterLiteral.FallTrigger);
    public readonly int Id_ReviveAnimationParameter = Animator.StringToHash(PlayerAnimParmaeterLiteral.ReviveTrigger);

    protected override void Awake()
    {
        base.Awake();
    }

    // ÄÁÆ®·Ñ·¯ ¸ØÃã
    public void StopController()
    {
        StopControll = true;
    }
    
    // ÄÁÆ®·Ñ·¯ ½ÇÇà
    public void PlayController()
    {
        StopControll = false;
    }

    public void Initialize()
    {
        _data = GetComponent<PlayerData>();
        _health = GetComponent<Health>();
        _animator = GetComponentInChildren<Animator>();
        _rig2D = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _horizontalMovement = GetComponent<PlayerHorizontalMovement>();
        _weaponHand = GetComponentInChildren<WeaponHand>();
        _data.Initialize();
        _health.Initialize(_data.Status.MaxHp);

        // Äü½½·Ô ¿¬µ¿
        UIBinder.Instance.BindQuickSlot(_weaponHand);
        SubscribeEvents();
    }

    public void SubscribeEvents()
    {
        _health.OnDie -= OnDie;
        _health.OnDie += OnDie;
        _health.OnRevive -= OnRevive;
        _health.OnRevive += OnRevive;
        _health.OnHit -= OnHit;
        _health.OnHit += OnHit;


        _health.OnHit -= GameManager.Instance.CameraManager.Effecter.PlayScreenShake;
        _health.OnHit += GameManager.Instance.CameraManager.Effecter.PlayScreenShake;
    }

    private void OnHit()
    {
        SoundManager.Instance.EffectPlay("Hit_Player", transform.position);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(IncreaseDashCount());

        //Initialize();
    }

    protected void Update()
    {
        if (StopControll == true)
        {
            _animator.SetTrigger(Id_IdleAnimationParameter);
        }
        if (_isDie == true) return;

        CheckRayAll();

        if(StopControll == false)
        {
            Flip();
        }


        if (CollisionInfo.IsSlope)
        {
            Rig2D.velocity = Vector2.zero;
        }
    }

    protected void LateUpdate()
    {
        if (_bounds != null)
            CharacterMovementBoundaryCheck();
    }

    private void DirectionUpdate()
    {
        Vector2 inputVec = _input.InputVec;
        if (inputVec.x < 0)
            _direction.x = -1;
        else if (inputVec.x > 0)
            _direction.x = 1;
        else
            _direction.x = 0;

        _faceDirection.x = Mathf.Sign(transform.localScale.x);
    }

    private void Flip()
    {
        Vector2 scale = transform.localScale;
        scale.x = transform.IsMouseOnLeft() switch
        {
            true => -1,
            false => 1
        };
        transform.localScale = scale;
        DirectionUpdate();
    }


    public IEnumerator IncreaseDashCount()
    {
        while (true)
        {
            _data.CurrentDashCount = Mathf.Clamp(_data.CurrentDashCount + 1, 0, _data.MaxDashCount);
            CanDash = _data.CurrentDashCount != 0;
            OnDashAction?.Invoke(_data.CurrentDashCount, _data.MaxDashCount);
            yield return YieldCache.WaitForSeconds(PlayerData.DEFAULT_DASH_COUNT_INTERVAL);
        }
    }

    public void DecreaseDashCount()
    {
        _data.CurrentDashCount = Mathf.Clamp(_data.CurrentDashCount - 1, 0, _data.MaxDashCount);
        CanDash = _data.CurrentDashCount != 0;
        OnDashAction?.Invoke(_data.CurrentDashCount, _data.MaxDashCount);
    }

    public void OnDie()
    {
        _animator.SetTrigger(PlayerAnimParmaeterLiteral.DieTrigger);
    }

    public void OnRevive()
    {
        IsRevive = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // ¾ÆÀÌÅÛ ¸Ô¾úÀ» ¶§ Ã³¸®
        IPickupable pickup = collision.gameObject.GetComponent<IPickupable>();
        pickup?.Pickup(_data);
    }
}
