using UnityEngine;

public class PlayerRevive : StateMachineBehaviour
{
    private PlayerController _controller;
    private Health _health;
    private PlayerData _data;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller == null)
        {
            _controller = animator.GetComponentInParent<PlayerController>();
            _health = _controller.GetComponent<Health>();
        }

        if (_data == null)
        {
            _data = animator.GetComponentInParent<PlayerData>();
        }
        _health.Revive();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetTrigger(_controller.Id_IdleAnimationParameter);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
