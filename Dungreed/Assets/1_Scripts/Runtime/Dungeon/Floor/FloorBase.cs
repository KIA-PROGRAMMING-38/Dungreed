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
    private bool _initializedPlayerObject = false;

    public virtual void OnPlayerDie()
    {
        _owner.PlayerDieProcess();
    }

    protected virtual void Start()
    {
        _player = GameManager.Instance.Player;
        ChangeRoom(_startRoom);
        GameManager.Instance.CameraManager.SetConfiner(_startRoom.RoomBounds);
        GameManager.Instance.Player.transform.position = _startRoom.StartPosition.Position;

        if(_initializedPlayerObject == false)
        {
            _player.GetComponent<Health>().OnDie -= OnPlayerDie;
            _player.GetComponent<Health>().OnDie += OnPlayerDie;
        }
    }

    protected virtual void OnEnable()
    {
        _player = GameManager.Instance.Player;
        _initializedPlayerObject = _player == null ? false : true;

        if (_initializedPlayerObject == true)
        {
            _player.GetComponent<Health>().OnDie -= OnPlayerDie;
            _player.GetComponent<Health>().OnDie += OnPlayerDie;
        }
    }

    protected virtual void OnDisable()
    {
        if(_player != null)
        {
            _player.GetComponent<Health>().OnDie -= OnPlayerDie;
        }
    }

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
