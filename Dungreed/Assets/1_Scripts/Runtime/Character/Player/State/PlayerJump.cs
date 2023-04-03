using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerJump : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = _controller ?? animator.GetComponentInParent<PlayerController>();
        _data = _data ?? animator.GetComponentInParent<PlayerData>();
        _data.JumpTimeCounter = _data.JumpTime;
        _data.IsJumping = true;
        _controller.Rig2D.velocity = new Vector2(_controller.Rig2D.velocity.x, _data.JumpForce);

        CreateJumpFx();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller.Rig2D.velocity.y < 0)
        {
            animator.SetTrigger(_controller.Id_FallAnimationParameter);
            return;
        }

        if (Input.GetMouseButtonDown(1) && _data.CanDash)
        {
            animator.SetTrigger(_controller.Id_DashAnimationParameter);
            return;
        }

        if (Input.GetKey(KeyCode.Space) && _data.IsJumping == true)
        {
            if (_data.JumpTimeCounter > 0)
            {
                _controller.Rig2D.velocity = new Vector2(_controller.Rig2D.velocity.x, _data.JumpForce);
                _data.JumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _data.IsJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _data.IsJumping = false;
        }
        
        _controller.HorizontalMovement.HorizontalMove();
    }

    void CreateJumpFx()
    {
        Vector2 pos = _controller.BoundCenter;
        pos.y = _controller.BoundCenter.y;
        GameManager.Instance.FxPooler.GetFx("JumpFx", pos, Quaternion.identity);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}
