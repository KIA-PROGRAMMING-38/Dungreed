public class Floor : FloorBase
{
    public override void Initialize()
    {
        foreach (RoomBase room in _rooms)
        {
            room.Floor = this;
            room.Initialize();
        }
    }

    public override void OnFloorEnter()
    {
        foreach (RoomBase room in _rooms)
        {
            room.Floor = this;
        }
        _player = GameManager.Instance.Player;
        ChangeRoom(_startRoom);
        GameManager.Instance.CameraManager.SetConfiner(_startRoom.RoomBounds);

        _player.transform.position = _startRoom.StartPosition.Position;
        _player.GetComponent<Health>().OnDie -= OnPlayerDie;
        _player.GetComponent<Health>().OnDie += OnPlayerDie;
    }

    public override void OnFloorStay()
    {
        _currentRoom?.OnRoomStay();
    }

    public override void OnFloorExit()
    {
        _player.GetComponent<Health>().OnDie -= OnPlayerDie;
    }

}

