using UnityEngine;

public class PlayerRun : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;

    private Vector2 _fxSpawnPosition;
    private float _moveFxSpawnInterval;
    private float _time;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _moveFxSpawnInterval = PlayerData.MOVE_FX_SPAWN_INTERVAL;

        if (_controller == null)
        {
            _controller = animator.GetComponentInParent<PlayerController>();
        }
        if (_data == null)
        {
            _data = animator.GetComponentInParent<PlayerData>();
        }

        CreateMoveFx();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller.Rig2D.velocity.y < 0 && _controller.CollisionInfo.IsGrounded == false)
        {
            animator.SetTrigger(_controller.Id_FallAnimationParameter);
            return;
        }

        if (Input.GetMouseButtonDown(1) && _controller.CanDash)
        {
            animator.SetTrigger(_controller.Id_DashAnimationParameter);
            return;
        }

        if (_controller.Input.X == 0)
        {
            animator.SetTrigger(_controller.Id_IdleAnimationParameter);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _controller.CollisionInfo.IsGrounded)
        {
            animator.SetTrigger(_controller.Id_JumpAnimationParameter);
            return;
        }

        _controller.HorizontalMovement.HorizontalMove();
        _time -= Time.deltaTime;
        if(_time <= 0f)
        {
            CreateMoveFx();
        }
    }

    void CreateMoveFx()
    {
        if (!(_controller.CollisionInfo.IsGrounded || _controller.CollisionInfo.IsOnewayGrounded)) return;
        if(_controller.CollisionInfo.left || _controller.CollisionInfo.right) return;
        _time = _moveFxSpawnInterval;
        _fxSpawnPosition = _controller.BoundCenter;
        _fxSpawnPosition.y = _controller.BottomBound;
        Vector3 newScale = new(Mathf.Sign(_controller.Input.X), 1, 1);
        GameManager.Instance.FxPooler.GetFx("MoveFx", _fxSpawnPosition, Quaternion.identity, newScale);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time = _moveFxSpawnInterval;
    }
}
