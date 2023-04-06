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
    private FxObject _kickFx;
    private Material _hitMaterial;

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

        _hitMaterial = ResourceCache.GetResource<Material>("Materials/HitMaterial");

        CreateDashFx();
        _kickFx = CreateKickFx();
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
            animator.SetTrigger(_controller.Id_FallAnimationParameter);
            return;
        }

        _dashTime += Time.deltaTime;
        _dashFxTime += Time.deltaTime;


        Vector3 kickFxPosition = _controller.BoundCenter;
        kickFxPosition.y = _controller.BottomBound;
        _kickFx.transform.position = kickFxPosition;
    }

    public void CreateDashFx()
    {
        _dashFxTime = 0f;

        FxObject obj = GameManager.Instance.FxPooler.GetFx("AfterImageFx", _controller.transform.position, Quaternion.identity);
        obj.ChangeLayerOrder(0);
        SpriteRenderer render = obj.GetComponent<SpriteRenderer>();
        render.sprite = _controller.Renderer.sprite;

        // 대쉬할때 이미지 머테리얼 변경
        if (_hitMaterial is not null)
            render.material = _hitMaterial;
    }

    public FxObject CreateKickFx()
    {
        Vector3 SpawnPosition = _controller.BoundCenter;
        SpawnPosition.y = _controller.BottomBound;
        FxObject obj = GameManager.Instance.FxPooler.GetFx("KickFx", SpawnPosition, Quaternion.identity, _controller.transform.localScale);
        obj.ChangeLayerOrder(2);
        return obj;
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 vel = _controller.Rig2D.velocity;
        vel.x = 0f;
        _controller.Rig2D.velocity = vel;
        _dashTime = 0f;
    }
}
