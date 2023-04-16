using UnityEngine;

public class EnemyController : BaseController
{
    private EnemyBase _enemy;
    private float _jumpForce;
    private float _jumpTime;
    private float _jumpElapsedTime;

    protected override void Awake()
    {
        base.Awake();
        _enemy = GetComponent<EnemyBase>();
        _jumpForce = PlayerData.DEFAULT_JUMP_FORCE * 1.5f;
        _jumpTime = PlayerData.DEFAULT_JUMP_TIME;
        _isDownJumping = false;
        _isJumping = false;
    }

    protected virtual void OnEnable() 
    {
        _isJumping = false;
        _isDownJumping = false;
        _jumpElapsedTime = 0f;
    }

    protected virtual void OnDisable()
    {

    }

    void Update()
    {
        CheckRayAll();

        if (_isJumping)
        {
            _jumpElapsedTime += Time.deltaTime;
            Debug.Log(_jumpElapsedTime);
            if (_jumpElapsedTime > _jumpTime)
            {
                _isJumping = false;
                _jumpElapsedTime = 0f;
            }
        }

        if (_isJumping && Rig2D.velocity.y < 0 && (CollisionInfo.IsGrounded || CollisionInfo.IsOnewayGrounded))
        {
            _isJumping = false;
            _jumpElapsedTime = 0f;
        }
    }

    public void LateUpdate()
    {
        if(_bounds != null)
            CharacterMovementBoundaryCheck();
    }

    public void HorizontalMove(ref Vector2 vel, float dirX)
    {
        _direction.x = dirX;
        if ((dirX == -1 && CollisionInfo.left == true) || dirX == -1 && CollisionInfo.right == true)
        {
            vel.x = 0f;
            SetFaceDirection(dirX);
            return;
        }

        SetFaceDirection(dirX);
        vel.x = _enemy.Data.MoveSpeed * dirX;
    }

    public void VerticalMove(ref Vector2 vel, float dirY)
    {
        float y = dirY;
        vel.y = _enemy.Data.MoveSpeed * y;
    }

    public void Jump(ref Vector2 vel)
    {
        if (_isJumping == true) return;
        if (_isDownJumping == true) return;

        _isJumping = true;
        vel.y = _jumpForce;
    }

    public void DownJump()
    {
        if (_isJumping == true) return;
        if (_isDownJumping == true) return;

        if (CollisionInfo.IsOnewayGrounded)
        {
            StartCoroutine(DisableCoroutine);
        }
    }

    public void SetFaceDirection(float dirX)
    {
        _faceDirection.x = dirX;
        Vector3 newScale = Vector3.one;
        newScale.x = dirX;
        transform.localScale = newScale;
    }
}
