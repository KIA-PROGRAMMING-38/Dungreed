using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

public class BossBelialHand : MonoBehaviour
{
    [SerializeField] private BossBelial _body;
    [SerializeField] private BelialLaser _laser;

    [SerializeField] private float _attackWaitTime;
    [SerializeField] private float _trackingTime;
    private Animator _anim;
    public BossBelial MainBody { get { return _body; } }
    public bool IsUseHand { get; private set; }

    public event Action OnDieAction;

    private static readonly int ID_AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int ID_ResetTrigger = Animator.StringToHash("Reset");

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        _laser.Initialize(damage: 9);
    }

    private void OnEnable()
    {
        _laser.OnLaserFireEnd -= OffUseHand;
        _laser.OnLaserFireEnd += OffUseHand;
        _body.OnDieAction -= OnDie;
        _body.OnDieAction += OnDie;
    }

    private void OnDisable()
    {
        _laser.OnLaserFireEnd -= OffUseHand;
        _body.OnDieAction -= OnDie;
    }

    private void OffUseHand()
    {
        IsUseHand = false;
    }

    private void OnDie()
    {
        OnDieAction?.Invoke();
        _anim.SetTrigger(ID_ResetTrigger);
    }

    public void Attack()
    {
        if (IsUseHand) return;

        IsUseHand = true;
        AttackTask().Forget();
    }

    public void LaserFire()
    {
        if (_body.isDie == false)
        {
            _laser.Fire();
        }
    }

    public async UniTaskVoid AttackTask()
    {
        GameObject player = GameManager.Instance.Player;
        BoxCollider2D col2D = player.GetComponent<BoxCollider2D>();

        float t = 0f;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = col2D.bounds.center;
        // 플레이어 위치로 이동
        while (t < _trackingTime)
        {
            t += Time.deltaTime;
            float ratio = t / _trackingTime;
            Vector2 newPosition = transform.position;
            newPosition.y = Mathf.Lerp(startPosition.y, targetPosition.y, ratio);
            transform.position = newPosition;
            await UniTask.Yield();
        }
        // 기다린 후
        await UniTask.Delay((int)(1000 * _attackWaitTime));
        _anim.SetTrigger(ID_AttackTrigger);

        while (IsUseHand == true)
        {
            await UniTask.Yield();
        }

    }
}
