using UnityEngine;

public class PlayerIdle : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;
    
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

        Vector2 vel = _controller.Rig2D.velocity;
        vel.x = 0;
        _controller.Rig2D.velocity = vel;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller.StopControll == true) return;

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && _controller.CollisionInfo.IsOnewayGrounded)
        {
            animator.SetTrigger(_controller.Id_FallAnimationParameter);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _controller.CollisionInfo.IsGrounded)
        {
            animator.SetTrigger(_controller.Id_JumpAnimationParameter);
            return;
        }
        if (_controller.Input.X != 0)
        {
            animator.SetTrigger(_controller.Id_RunAnimationParameter);
            return;
        }

        if (Input.GetMouseButtonDown(1) && _controller.CanDash)
        {
            animator.SetTrigger(_controller.Id_DashAnimationParameter);
            return;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
