using UnityEngine;

public class PlayerDash : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;

    
    private float _dashFxInterval = 0.03f;
    private float _dashTime = 0f;
    private float _dashFxTime = 0f;
    private Vector2 _dir;
    private Vector2 _Force;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = _controller ?? animator.GetComponentInParent<PlayerController>();
        _data = _data ?? animator.GetComponentInParent<PlayerData>();

        _controller.DecreaseDashCount();
        _controller.Rig2D.velocity = Vector2.zero;

        _dashTime = 0f;
        _dir = _data.transform.position.MouseDir();
        _Force = _dir * _data.DashPower;
        _controller.Rig2D.velocity = _Force;

        if (_controller.CollisionInfo.IsOnewayGrounded)
            _controller.StartCoroutine(_controller.DisableCollision());
        CreateDashFx();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _controller.Rig2D.velocity = _Force;
        
        if(_dashFxTime >= _dashFxInterval)
        {
            CreateDashFx();
            _dashFxTime = 0f;
        }

        if (_dashTime >= _data.DashTime)
        {
            animator.SetTrigger(_controller.Id_IdleAnimationParameter);
            return;
        }

        _dashTime += Time.deltaTime;
        _dashFxTime += Time.deltaTime;
    }

    public void CreateDashFx()
    {
        _dashFxTime = 0f;
        SpriteRenderer render = GameManager.Instance.FxPooler.GetFx("AfterImageFx", _controller.transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
        render.sprite = _controller.Renderer.sprite;


        GameManager.Instance.FxPooler.GetFx("AfterImageFx", _controller.transform.position, Quaternion.identity);
    }

    public void CreateKickFx()
    {
        Vector3 SpawnPosition = _controller.BoundCenter;
        SpawnPosition.y = _controller.BottomBound;
        FxObject obj = GameManager.Instance.FxPooler.GetFx("KickFx", SpawnPosition, Quaternion.identity, _controller.transform.localScale);
        obj.ChangeLayerOrder(2);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CreateKickFx();
        Vector2 vel = _controller.Rig2D.velocity;
        vel.x = 0f;
        _controller.Rig2D.velocity = vel;
        _dashTime = 0f;
    }
}
