using UnityEngine;

public struct FloorInformation
{ 

}

public abstract class FloorBase : MonoBehaviour
{
    protected DungeonManager _owner;
    public DungeonManager Owner { protected get { return _owner; } set { _owner = value; } }

    protected GameObject _player;
    public GameObject CurrentPlayer { get { return _player; } }

    [SerializeField] protected RoomBase[] _rooms;

    [SerializeField] protected StartRoom _startRoom;
    [ShowOnly, SerializeField] protected RoomBase _currentRoom;

    [ShowOnly, SerializeField] protected FloorInformation floorInfo;
    protected bool _initializedPlayerObject = false;

    public virtual void OnPlayerDie()
    {
        _owner.PlayerDieProcess();
    }

    public abstract void Initialize();
    public abstract void OnFloorEnter();
    public abstract void OnFloorStay();
    public abstract void OnFloorExit();

    public void ChangeRoom(RoomBase nextRoom)
    {
        if (nextRoom.Equals(_currentRoom))
        {
            return;
        }

        if (_currentRoom == null)
        {
            _currentRoom = nextRoom;
            _currentRoom.OnRoomEnter();
            return;
        }

        _currentRoom.OnRoomExit();
        _currentRoom = nextRoom;
        _currentRoom.OnRoomEnter();
    }
}
