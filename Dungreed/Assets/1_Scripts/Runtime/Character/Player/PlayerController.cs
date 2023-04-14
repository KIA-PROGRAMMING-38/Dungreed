
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    #region Components
    private Health _health;
    private PlayerData _data;
    private PlayerInput _input;
    private Animator _animator;
    private SpriteRenderer _renderer;

    private PlayerHorizontalMovement _horizontalMovement;

    public Animator Anim { get { return _animator; } }
    public PlayerInput Input { get { return _input; } }
    public SpriteRenderer Renderer { get { return _renderer; } }
    public PlayerHorizontalMovement HorizontalMovement { get { return _horizontalMovement; } }

    public event Action<int, int> OnDashAction;
    #endregion


    public readonly int Id_DieAnimationParameter    = Animator.StringToHash(PlayerAnimParmaeterLiteral.DieTrigger);
    public readonly int Id_DashAnimationParameter   = Animator.StringToHash(PlayerAnimParmaeterLiteral.DashTrigger);
    public readonly int Id_JumpAnimationParameter   = Animator.StringToHash(PlayerAnimParmaeterLiteral.JumpTrigger);
    public readonly int Id_RunAnimationParameter    = Animator.StringToHash(PlayerAnimParmaeterLiteral.RunTrigger);
    public readonly int Id_IdleAnimationParameter   = Animator.StringToHash(PlayerAnimParmaeterLiteral.IdleTrigger);
    public readonly int Id_FallAnimationParameter   = Animator.StringToHash(PlayerAnimParmaeterLiteral.FallTrigger);
    public readonly int Id_ReviveAnimationParameter = Animator.StringToHash(PlayerAnimParmaeterLiteral.ReviveTrigger);

    public IEnumerator DisableCoroutine;

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

        // TODO: Delete
        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
        DisableCoroutine = DisableCollision();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(IncreaseDashCount());
    }

    protected void OnEnable()
    {
        _health.OnDie -= OnDie;
        _health.OnDie += OnDie;
    }

    protected void OnDisable()
    {
        _health.OnDie -= OnDie;
    }

    protected void Update()
    {
        CheckRayAll();
        Flip();


        if (CollisionInfo.IsSlope)
        {
            Rig2D.velocity = Vector2.zero;
        }
    }

    protected void LateUpdate()
    {
        if(_bounds != null)
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

    public IEnumerator DisableCollision()
    {
        while(true)
        {
            var platformCol = _onewayPlatformCollider;
            Physics2D.IgnoreCollision(_collider, platformCol);
            yield return YieldCache.WaitForSeconds(0.25f);
            Physics2D.IgnoreCollision(_collider, platformCol, false);
            _onewayPlatformCollider = null;

            StopCoroutine(DisableCoroutine);
            yield return null;
        }
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
        Debug.Log("Á×À½");
        _animator.SetTrigger(PlayerAnimParmaeterLiteral.DieTrigger);
    }
}
