using UnityEngine;

public class PlayerFall : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;
    private static readonly string _jumpSoundName = "Jump";

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

        if (true == _controller.CollisionInfo.IsOnewayGrounded)
        {
            _controller.DisableCollision().Forget();
            SoundManager.Instance.EffectPlay(_jumpSoundName, animator.transform.position);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller.CollisionInfo.IsGrounded == true || _controller.CollisionInfo.IsOnewayGrounded)
        {
            animator.SetTrigger(_controller.Id_IdleAnimationParameter);
        }


        if (Input.GetMouseButtonDown(1) && _controller.CanDash)
        {
            animator.SetTrigger(_controller.Id_DashAnimationParameter);
            return;
        }

        _controller.HorizontalMovement.HorizontalMove();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
