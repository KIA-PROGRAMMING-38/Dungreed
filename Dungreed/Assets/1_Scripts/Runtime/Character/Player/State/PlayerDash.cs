using UnityEngine;

public class PlayerDash : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;
    float t = 0f;
    Vector2 dir;
    Vector2 Force;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = _controller ?? animator.GetComponentInParent<PlayerController>();
        _data = _data ?? animator.GetComponentInParent<PlayerData>();

        _controller.DecreaseDashCount();

        _controller.Rig2D.velocity = Vector2.zero;

        t = _data.DashTime;
        dir = ((Vector3)Utils.Utility2D.GetMousePosition() - _data.transform.position).normalized;
        Force = dir * _data.DashPower;
        _controller.Rig2D.velocity = Force;
        // _controller.ghost.Initialize();
        // _controller.ghost.makeGhost = true;

        if (_controller.CollisionInfo.IsOnewayGrounded)
            _controller.StartCoroutine(_controller.DisableCollision());
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (t < 0f)
        {
            //_controller.RollbackState();
            animator.SetTrigger(_controller.Id_IdleAnimationParameter);
            return;
        }
        _controller.Rig2D.velocity = Force;
        t -= Time.deltaTime;
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller.Rig2D.velocity = Vector3.zero;
        //_controller.ghost.Initialize();
    }
}
