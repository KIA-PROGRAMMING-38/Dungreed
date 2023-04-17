using System;
using UnityEngine;


public struct RoomInfo
{
    public int EnemyCount;
    public int EnemyDieCount;
    public bool Clear { get { return EnemyCount == EnemyDieCount; } }
}

[RequireComponent(typeof(LevelBounds))]
public abstract class RoomBase : MonoBehaviour
{
    // Room Info

    // Enemy Count
    // private Enemies ...
    private EnemyBase[] _enemies;

    public FloorBase Floor { get; set; }
    public LevelBounds RoomBounds { get; protected set; }

    [SerializeField] 
    protected RoomConnector[] _roomEntrance;

    [ShowOnly, SerializeField]
    protected RoomInfo _info;
    public RoomInfo Info { get { return _info; } }

    public event Action OnRoomClear;

    protected virtual void Awake()
    {
        RoomBounds = GetComponent<LevelBounds>();
        _enemies = GetComponentsInChildren<EnemyBase>();
        Debug.Assert(RoomBounds != null);
    }

    protected virtual void OnEnable()
    {
        _info.EnemyCount = _enemies.Length;

        foreach (EnemyBase enemy in _enemies)
        {
            enemy.OnDie -= AddEnemyDieCount;
            enemy.OnDie += AddEnemyDieCount;
        }
    }

    protected virtual void OnDisable()
    {
        foreach (EnemyBase enemy in _enemies)
        {
            enemy.OnDie -= AddEnemyDieCount;
        }
    }

    protected virtual void Start()
    {
        if (_enemies.Length == 0)
        {
            OnRoomClear?.Invoke();
        }
    }

    private void AddEnemyDieCount()
    {
        _info.EnemyDieCount++;

        if(_info.Clear == true)
        {
            OnRoomClear?.Invoke();
        }
    }

    public abstract void OnRoomEnter();
    public abstract void OnRoomStay();
    public abstract void OnRoomExit();
}
