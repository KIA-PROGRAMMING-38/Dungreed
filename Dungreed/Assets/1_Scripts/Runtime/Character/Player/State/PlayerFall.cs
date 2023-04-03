using UnityEngine;

public class PlayerFall : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = _controller ?? animator.GetComponentInParent<PlayerController>();
        _data = _data ?? animator.GetComponentInParent<PlayerData>();
        Debug.Log("Fall State");
        if(true == _controller.CollisionInfo.IsOnewayGrounded)
        {
            Debug.Log("OnewayFall State");
            _controller.StartCoroutine(_controller.DisableCollision());
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller.CollisionInfo.IsGrounded == true || _controller.CollisionInfo.IsOnewayGrounded)
        {
            animator.SetTrigger(_controller.Id_IdleAnimationParameter);
        }


        if (Input.GetMouseButtonDown(1) && _data.CanDash)
        {
            animator.SetTrigger(_controller.Id_DashAnimationParameter);
        }

        _controller.HorizontalMovement.HorizontalMove();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
