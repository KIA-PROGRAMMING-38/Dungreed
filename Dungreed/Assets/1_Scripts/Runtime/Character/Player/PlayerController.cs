
using System.Collections;
using UnityEngine;

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


    public readonly int Id_DieAnimationParameter    = Animator.StringToHash(PlayerAnimParmaeterLiteral.DieTrigger);
    public readonly int Id_DashAnimationParameter   = Animator.StringToHash(PlayerAnimParmaeterLiteral.DashTrigger);
    public readonly int Id_JumpAnimationParameter   = Animator.StringToHash(PlayerAnimParmaeterLiteral.JumpTrigger);
    public readonly int Id_RunAnimationParameter    = Animator.StringToHash(PlayerAnimParmaeterLiteral.RunTrigger);
    public readonly int Id_IdleAnimationParameter   = Animator.StringToHash(PlayerAnimParmaeterLiteral.IdleTrigger);
    public readonly int Id_FallAnimationParameter   = Animator.StringToHash(PlayerAnimParmaeterLiteral.FallTrigger);


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
        Transform.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(IncreaseDashCount());
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
        var platformCol = _onewayPlatformCollider;
        Physics2D.IgnoreCollision(_collider, platformCol);
        yield return YieldCache.WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(_collider, platformCol, false);
        _onewayPlatformCollider = null;
    }
    public IEnumerator IncreaseDashCount()
    {
        while (true)
        {
            _data.CurrentDashCount = Mathf.Clamp(_data.CurrentDashCount + 1, 0, _data.MaxDashCount);
            CanDash = _data.CurrentDashCount != 0;
            yield return YieldCache.WaitForSeconds(PlayerData.DEFAULT_DASH_COUNT_INTERVAL);
        }
    }

    public void DecreaseDashCount()
    {
        _data.CurrentDashCount = Mathf.Clamp(_data.CurrentDashCount - 1, 0, _data.MaxDashCount);
        CanDash = _data.CurrentDashCount != 0;
    }
}
