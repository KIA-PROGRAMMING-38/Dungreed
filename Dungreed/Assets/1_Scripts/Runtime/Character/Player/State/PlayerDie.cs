using UnityEngine;

public class PlayerDie : StateMachineBehaviour
{
    private PlayerController _controller;
    private Health _health;
    private PlayerData _data;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller == null)
        {
            _controller = animator.GetComponentInParent<PlayerController>();
            _health = _controller.GetComponentAllCheck<Health>();
        }
        if (_data == null)
        {
            _data = animator.GetComponentInParent<PlayerData>();
        }
        
        _controller.IsDie = true;
        _health.IsInvincible = true;
        _controller.Hand.gameObject.SetActive(false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller.IsDie = false;
        _health.IsInvincible = false;
        _controller.Hand.gameObject.SetActive(true);
    }
}
