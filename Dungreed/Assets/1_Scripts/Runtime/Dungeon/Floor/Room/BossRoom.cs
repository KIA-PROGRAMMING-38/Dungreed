using UnityEngine;

public class BossRoom : RoomBase
{
    [SerializeField] private InitPosition _startPosition;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void OnRoomEnter()
    {
        _player.transform.position = _startPosition.Position;
        GameManager.Instance.CameraManager.SettingCamera(RoomBounds, _startPosition);
        _player.transform.position = _startPosition.Position;
    }

    public override void OnRoomExit()
    {

    }

    public override void OnRoomStay()
    {

    }
}
