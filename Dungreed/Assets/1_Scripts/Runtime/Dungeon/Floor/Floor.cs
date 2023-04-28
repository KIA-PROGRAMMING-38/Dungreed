using UnityEngine;

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
        SoundManager.Instance.BGMPlay(_backgroundMusic);
        GameManager.Instance.CameraManager.Effecter.PlayTransitionEffect(null, false);
        GameManager.Instance.CameraManager.SetConfiner(_startRoom.RoomBounds);
        _player.transform.position = _startRoom.StartPosition.Position;
        _player.GetComponent<PlayerController>().SetBounds(_startRoom.RoomBounds);
        GameManager.Instance.CameraManager.VirtualCamera.ForceCameraPosition(_player.transform.position, Quaternion.identity);
        GameManager.Instance.CameraManager.VirtualCamera.m_Follow = _player.transform;
    }

    public override void OnFloorStay()
    {
        _currentRoom?.OnRoomStay();
    }

    public override void OnFloorExit()
    {
    }

}

