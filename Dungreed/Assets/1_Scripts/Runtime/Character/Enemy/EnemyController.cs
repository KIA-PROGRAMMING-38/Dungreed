using UnityEngine;

public class EnemyController : BaseController
{

    private bool _isDownJumping;
    private float _jumpForce;
    private float _jumpTime;
    private float _jumpElapsedTime;
    private Vector2 _velocity;

    protected override void Awake()
    {
        base.Awake();
        _jumpForce = PlayerData.DEFAULT_JUMP_FORCE * 1.5f;
        _jumpTime = PlayerData.DEFAULT_JUMP_TIME;
        _isDownJumping = false;
        _isJumping = false;
    }

    void Update()
    {
        CheckRayAll();
        if (CollisionInfo.IsGrounded)
        {
            _isJumping = false;
            _jumpElapsedTime = 0f;
            _velocity.y = 0f;
        }

        _velocity = Rig2D.velocity;

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Enemy Jump");
            Jump(ref _velocity);
        }

        if (_isJumping)
        {
            Debug.Log(_jumpElapsedTime);
            _jumpElapsedTime += Time.deltaTime;
            if(_jumpElapsedTime > _jumpTime )
            {
                _isJumping = false;
                _jumpElapsedTime = 0f;
            }
        }
       

        Rig2D.velocity = _velocity;
    }

    public void Jump(ref Vector2 vel)
    {
        if(CollisionInfo.IsGrounded)
        {
            _isJumping = true;
            vel.y = _jumpForce;
        }
    }

    public void DownJump()
    {
        if (CollisionInfo.IsOnewayGrounded)
            StartCoroutine(DisableCoroutine);
    }
}
