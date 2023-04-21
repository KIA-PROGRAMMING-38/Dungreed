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

    public bool IsActPattern { get => _isActPattern; }

    protected GameObject _player;

    public virtual void Initialize(BossRoom _room)
    {
        _ownerRoom = _room;

        _anim = GetComponent<Animator>();
        _health = GetComponent<Health>();

        _health.OnDie -= OnDie;
        _health.OnDie += OnDie;
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
        // _ownerRoom.IsBossCleared = true;
    }
}
