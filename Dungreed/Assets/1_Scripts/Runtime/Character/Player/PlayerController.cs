
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

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
    #endregion

    public static readonly string DieAnimParam = "Die";
    public static readonly string JumpAnimParam = "Jump";
    public static readonly string DashAnimParam = "Dash";
    public static readonly string RunAnimParam = "Run";
    public static readonly string IdleAnimParam = "Idle";
    public static readonly string FallAnimParam = "Fall";

    public readonly int Id_DieAnimationParameter = Animator.StringToHash(DashAnimParam);
    public readonly int Id_DashAnimationParameter = Animator.StringToHash(DashAnimParam);
    public readonly int Id_JumpAnimationParameter = Animator.StringToHash(JumpAnimParam);
    public readonly int Id_RunAnimationParameter = Animator.StringToHash(RunAnimParam);
    public readonly int Id_IdleAnimationParameter = Animator.StringToHash(IdleAnimParam);
    public readonly int Id_FallAnimationParameter = Animator.StringToHash(FallAnimParam);


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
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(IncreaseDashCount());
    }

    void Update()
    {
        CheckRayAll();
        Flip();

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
        Physics2D.IgnoreCollision(_collider, _onewayPlatformCollider);
        yield return YieldCache.WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(_collider, _onewayPlatformCollider, false);
        _onewayPlatformCollider = null;
    }
    public IEnumerator IncreaseDashCount()
    {
        while (true)
        {
            _data.CurrentDashCount = Mathf.Clamp(_data.CurrentDashCount + 1, 0, _data.MaxDashCount);
            _data.CanDash = _data.CurrentDashCount != 0;
            yield return YieldCache.WaitForSeconds(_data.DashCountInterval);
        }
    }
    public void DecreaseDashCount()
    {
        _data.CurrentDashCount = Mathf.Clamp(_data.CurrentDashCount - 1, 0, _data.MaxDashCount);
        _data.CanDash = _data.CurrentDashCount != 0;
    }
}
