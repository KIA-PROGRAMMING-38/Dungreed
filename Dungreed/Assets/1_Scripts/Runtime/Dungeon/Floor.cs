using UnityEngine;

public class Floor : MonoBehaviour
{
    private GameObject _player;
    [SerializeField]  private RoomBase[] _rooms;

    [SerializeField] private StartRoom _startRoom;
    [ShowOnly, SerializeField] private RoomBase _currentRoom;

    private void Awake()
    {
        foreach(RoomBase room in _rooms)
        {
            room.Floor = this;
        }
    }

    private void Start()
    {
        ChangeRoom(_startRoom);
        GameManager.Instance.CameraManager.SetConfiner(_startRoom.RoomBounds);
        GameManager.Instance.Player.transform.position = _startRoom.StartPosition.Position;
    }

    void Update()
    {
        _currentRoom?.RoomUpdate();
    }

    public void ChangeRoom(RoomBase nextRoom)
    {
        if(nextRoom.Equals(_currentRoom))
        {
            return;
        }

        if(_currentRoom == null)
        {
            _currentRoom = nextRoom;
            _currentRoom.RoomEnter();
            return;
        }

        _currentRoom.RoomExit();
        _currentRoom = nextRoom;
        _currentRoom.RoomEnter();
    }
}
