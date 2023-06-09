using UnityEngine;

public struct FloorInformation
{ 

}

public abstract class FloorBase : MonoBehaviour
{
    protected DungeonManager _owner;
    public DungeonManager Owner { protected get { return _owner; } set { _owner = value; } }
    [SerializeField] protected string _backgroundMusic;
    public string BackGroudMusic { get { return _backgroundMusic; } }
    protected GameObject _player;

    [SerializeField] protected RoomBase[] _rooms;

    [SerializeField] protected StartRoom _startRoom;
    [ShowOnly, SerializeField] protected RoomBase _currentRoom;

    [ShowOnly, SerializeField] protected FloorInformation floorInfo;
    protected bool _initializedPlayerObject = false;

    public void SetPlayer(GameObject player) => _player = player;

    public abstract void Initialize();
    public abstract void OnFloorEnter();
    public abstract void OnFloorStay();
    public abstract void OnFloorExit();


    public void OnPlayerDie()
    {
        _currentRoom.OnPlayerDie();
    }

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
