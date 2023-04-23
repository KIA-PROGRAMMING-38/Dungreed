public class Floor : FloorBase
{
    public override void Initialize()
    {
        foreach (RoomBase room in _rooms)
        {
            room.Floor = this;
            room.SetPlayer(_player);
            room.Initialize();
        }
    }

    public override void OnFloorEnter()
    {
        ChangeRoom(_startRoom);
        GameManager.Instance.CameraManager.Effecter.PlayTransitionEffect(null, false);
        GameManager.Instance.CameraManager.SetConfiner(_startRoom.RoomBounds);

        _player.transform.position = _startRoom.StartPosition.Position;
    }

    public override void OnFloorStay()
    {
        _currentRoom?.OnRoomStay();
    }

    public override void OnFloorExit()
    {
    }

}

