using UnityEngine;

public class PlayerJump : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;
    private float _jumpTimeCounter;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller == null)
        {
            _controller = animator.GetComponentInParent<PlayerController>();
        }
        if (_data == null)
        {
            _data = animator.GetComponentInParent<PlayerData>();
        }

        _jumpTimeCounter = PlayerData.DEFAULT_JUMP_TIME;
        _controller.IsJumping = true;
        _controller.Rig2D.velocity = new Vector2(_controller.Rig2D.velocity.x, _data.JumpForce);

        CreateJumpFx();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller.Rig2D.velocity.y < 0)
        {
            animator.SetTrigger(_controller.Id_FallAnimationParameter);
        }

        if (Input.GetMouseButtonDown(1) && _controller.CanDash)
        {
            animator.SetTrigger(_controller.Id_DashAnimationParameter);
            return;
        }

        if (Input.GetKey(KeyCode.Space) && _controller.IsJumping == true)
        {
            if (_jumpTimeCounter > 0)
            {
                _controller.Rig2D.velocity = new Vector2(_controller.Rig2D.velocity.x, _data.JumpForce);
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _controller.IsJumping = false;
                if(_controller.CollisionInfo.IsGrounded || _controller.CollisionInfo.IsOnewayGrounded)
                {
                    animator.SetTrigger(_controller.Id_IdleAnimationParameter);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _controller.IsJumping = false;
        }
        
        _controller.HorizontalMovement.HorizontalMove();
    }

    void CreateJumpFx()
    {
        Vector2 pos = _controller.BoundCenter;
        pos.y = _controller.BottomBound;
        GameManager.Instance.FxPooler.GetFx("JumpFx", pos, Quaternion.identity);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}
