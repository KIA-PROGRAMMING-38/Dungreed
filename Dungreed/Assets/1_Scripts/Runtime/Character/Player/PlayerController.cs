
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class PlayerController : BaseController
{
    private bool _isDie;
    public bool IsDie { get { return _isDie; } set { _isDie = value; } }
    public bool IsDashing { get; set; }
    public bool StopControll { get; private set; }
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

        _data = GetComponent<PlayerData>();
        _health = GetComponent<Health>();
        _animator = GetComponentInChildren<Animator>();
        _rig2D = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _horizontalMovement = GetComponent<PlayerHorizontalMovement>();
        _weaponHand = GetComponentInChildren<WeaponHand>();

        // TODO: Delete
        if (GameManager.Instance != null && GameManager.Instance.CameraManager!=null)
        {
            GameManager.Instance.CameraManager.VirtualCamera.Follow = transform;
        }
    }

    // 컨트롤러 멈춤
    public void StopController()
    {
        StopControll = true;
    }
    
    // 컨트롤러 실행
    public void PlayController()
    {
        StopControll = false;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(IncreaseDashCount());
        GameManager.Instance.CameraManager.VirtualCamera.Follow = transform;

        _health.OnDie -= OnDie;
        _health.OnDie += OnDie;
        _health.OnRevive -= OnRevive;
        _health.OnRevive += OnRevive;


        _health.OnHit -= GameManager.Instance.CameraManager.Effecter.PlayScreenShake;
        _health.OnHit += GameManager.Instance.CameraManager.Effecter.PlayScreenShake;
    }

    protected void Update()
    {
        if (StopControll == true)
        {
            _animator.SetTrigger(Id_IdleAnimationParameter);
        }
        if (_isDie == true) return;

        CheckRayAll();
        Flip();


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
        _animator.SetTrigger(PlayerAnimParmaeterLiteral.ReviveTrigger);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // 아이템 먹었을 때 처리
        IPickupable pickup = collision.gameObject.GetComponent<IPickupable>();
        pickup?.Pickup(_data);
    }
}
