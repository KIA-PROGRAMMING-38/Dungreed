using UnityEngine;

public class PlayerDash : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;

    
    float _dashFxInterval = 0.03f;
    float _dashTime = 0f;
    float _dashFxTime = 0f;
    Vector2 dir;
    Vector2 Force;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = _controller ?? animator.GetComponentInParent<PlayerController>();
        _data = _data ?? animator.GetComponentInParent<PlayerData>();

        _controller.DecreaseDashCount();

        _controller.Rig2D.velocity = Vector2.zero;

        _dashTime = 0f;
        dir = ((Vector3)Utils.Utility2D.GetMousePosition() - _data.transform.position).normalized;
        Force = dir * _data.DashPower;
        _controller.Rig2D.velocity = Force;

        if (_controller.CollisionInfo.IsOnewayGrounded)
            _controller.StartCoroutine(_controller.DisableCollision());

        CreateDashFx();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _controller.Rig2D.velocity = Force;
        
        _dashTime += Time.deltaTime;
        _dashFxTime += Time.deltaTime;
        if(_dashFxTime >= _dashFxInterval)
        {
            CreateDashFx();
            _dashFxTime = 0f;
        }

        if (_dashTime >= _data.DashTime)
        {
            animator.SetTrigger(_controller.Id_FallAnimationParameter);
            return;
        }
    }

    public void CreateDashFx()
    {
        _dashFxTime = 0f;
        SpriteRenderer render = GameManager.Instance.FxPooler.GetFx("AfterImageFx", _controller.transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
        render.sprite = _controller.Renderer.sprite;
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 vel = _controller.Rig2D.velocity;
        vel.x = 0f;
        _controller.Rig2D.velocity = vel;
    }
}
