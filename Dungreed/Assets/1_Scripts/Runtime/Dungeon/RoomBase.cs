using System;
using UnityEngine;

[RequireComponent(typeof(LevelBounds))]
public abstract class RoomBase : MonoBehaviour
{
    // Room Info

    // Enemy Count
    // private Enemies ...

    public Floor Floor { get; set; }
    public LevelBounds RoomBounds { get; protected set; }

    [SerializeField] 
    protected RoomConnector[] _roomEntrance;

    public Action OnRoomClear;

    protected virtual void Awake()
    {
        RoomBounds = GetComponent<LevelBounds>();
        Debug.Assert(RoomBounds != null);
    }

    public abstract void RoomEnter();
    public abstract void RoomUpdate();
    public abstract void RoomExit();
}
