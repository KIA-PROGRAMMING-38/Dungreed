using System;
using UnityEngine;

public struct RoomInfo
{ 
    
}

[RequireComponent(typeof(LevelBounds))]
public abstract class RoomBase : MonoBehaviour
{
    // Room Info

    // Enemy Count
    // private Enemies ...

    public FloorBase Floor { get; set; }
    public LevelBounds RoomBounds { get; protected set; }

    [SerializeField] 
    protected RoomConnector[] _roomEntrance;

    [ShowOnly, SerializeField]
    protected RoomInfo _roomInfo;

    public event Action OnRoomClear;

    protected virtual void Awake()
    {
        RoomBounds = GetComponent<LevelBounds>();
        Debug.Assert(RoomBounds != null);
    }

    public abstract void OnRoomEnter();
    public abstract void OnRoomStay();
    public abstract void OnRoomExit();
}
