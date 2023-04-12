using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDash : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;


    private float _dashFxInterval;
    private float _dashFxTime = 0f;
    private float _dashFxMaxTime = 0f;
    private float _dashTime = 0f;
    private Vector2 _dir;
    private Vector2 _Force;
    private FxObject _kickFx;
    private Material _hitMaterial;

    private float _colliderRadius = 0.7f;
    private Collider2D[] _hit;

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


        if (_hit == null)
        {
            _hit = new Collider2D[10];
        }

        _controller.DecreaseDashCount();
        _controller.Rig2D.velocity = Vector2.zero;

        _dashTime = 0f;
        _dashFxMaxTime = PlayerData.DEFAULT_DASH_TIME * 0.8f;
        _dashFxInterval = _dashFxMaxTime / 5f;

        _dir = _data.transform.position.MouseDir();
        _Force = _dir * PlayerData.DEFAULT_DASH_POWER;
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

        if (_dashTime >= PlayerData.DEFAULT_DASH_TIME)
        {
            animator.SetTrigger(_controller.Id_FallAnimationParameter);
            return;
        }

        if (_dashFxMaxTime >= _dashTime && _dashFxTime >= _dashFxInterval)
        {
            CreateDashFx();
            _dashFxTime = 0f;
        }

        _dashTime += Time.deltaTime;
        _dashFxTime += Time.deltaTime;


        Vector3 kickFxPosition = _controller.BoundCenter;
        kickFxPosition.y = _controller.BottomBound;
        _kickFx.transform.position = kickFxPosition;
    }

    // TODO: 캐릭터 능력치 대쉬 공격력 적용해야함
    private void DashAttack()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(_controller.transform.position, _colliderRadius, _hit);
        for (int i = 0; i < hitCount; i++)
        {
            IDamageable obj = _hit[i].GetComponent<IDamageable>();

            // TODO : obj?.Hit(캐릭터 대쉬 공격력, gameObject);
        }
    }

    public void CreateDashFx()
    {
        FxObject obj = GameManager.Instance.FxPooler.GetFx("AfterImageFx", _controller.transform.position, Quaternion.identity);
        // obj.ChangeLayerOrder(0);
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

        _dashFxInterval = 0f;
        _dashFxTime = 0f;
        _dashFxMaxTime = 0f;
        _dashTime = 0f;
        _dir = Vector2.zero;
        _Force = Vector3.zero;
    }
}
