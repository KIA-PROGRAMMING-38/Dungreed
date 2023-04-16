public class Floor : FloorBase
{
    private void Awake()
    {
        foreach (RoomBase room in _rooms)
        {
            room.Floor = this;
        }
    }

    public override void OnFloorEnter()
    {
        foreach (RoomBase room in _rooms)
        {
            room.Floor = this;
        }
    }

    public override void OnFloorStay()
    {
        _currentRoom?.OnRoomStay();
    }

    public override void OnFloorExit()
    {
    }

}

