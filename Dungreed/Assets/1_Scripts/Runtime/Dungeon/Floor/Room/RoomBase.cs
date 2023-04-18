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
    protected GameObject _player;
    protected EnemyBase[] _enemies;

    public FloorBase Floor { get; set; }
    public LevelBounds RoomBounds { get; protected set; }

    [SerializeField] 
    protected RoomConnector[] _roomConnectors;

    [ShowOnly, SerializeField]
    protected RoomInfo _info;
    public RoomInfo Info { get { return _info; } }

    public Action OnRoomClear;

    public void SetPlayer(GameObject player) => _player = player;

    public virtual void OnPlayerDie()
    {

    }

    public virtual void Initialize()
    {
        RoomBounds = GetComponent<LevelBounds>();
        Debug.Assert(RoomBounds != null);


        foreach(RoomConnector connector in _roomConnectors)
        {
            connector?.Initialize();
        }
    }

    protected virtual void OnDisable()
    {
    }


    public abstract void OnRoomEnter();
    public abstract void OnRoomStay();
    public abstract void OnRoomExit();
}
