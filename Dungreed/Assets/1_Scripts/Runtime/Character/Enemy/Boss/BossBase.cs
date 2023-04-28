using System;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    [SerializeField]
    protected EnemyData _enemyData;
    protected BossRoom _ownerRoom;
    protected Health _health;
    protected Animator _anim;

    protected bool _isDie;
    protected bool _isBattleStart;
    protected bool _isActPattern;
    protected static readonly string EnemyHitSoundName = "Hit_Enemy";

    public bool IsActPattern { get => _isActPattern; }
    public EnemyData EnemyData { get => _enemyData; }
    protected GameObject _player;

    public virtual void Initialize(BossRoom _room)
    {
        _ownerRoom = _room;

        _anim = GetComponent<Animator>();
        _health = GetComponent<Health>();

        _health.OnDie -= OnDie;
        _health.OnDie += OnDie;
        _health.OnHit -= OnHit;
        _health.OnHit += OnHit;

        // 배틀 시작 이벤트 구독
        _ownerRoom.OnBossBattleStart += OnBattleStart;
    }


    /// <summary>
    /// 플레이어를 찾습니다.
    /// </summary>
    protected virtual void FindPlayer()
    {
        if(GameManager.Instance != null && GameManager.Instance.Player != null)
        {
            _player = GameManager.Instance.Player;
        }
    }

    /// <summary>
    /// Boss의 Health -> CurrentHp가 0이 되었을 때에 호출됩니다.
    /// </summary>
    protected virtual void OnDie()
    {

    }

    protected virtual void OnHit()
    {
        SoundManager.Instance.EffectPlay(EnemyHitSoundName, transform.position);
    }

    protected virtual void OnBattleStart()
    {
        _isBattleStart = true;
    }
}
