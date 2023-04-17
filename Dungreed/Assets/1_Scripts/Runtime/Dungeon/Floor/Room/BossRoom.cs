using UnityEngine;

public class BossRoom : RoomBase
{
    [SerializeField] private InitPosition _startPosition;

    public override void OnRoomEnter()
    {
        Floor.CurrentPlayer.transform.position = _startPosition.Position;
        GameManager.Instance.CameraManager.SettingCamera(RoomBounds, _startPosition);
        Floor.CurrentPlayer.transform.position = _startPosition.Position;
    }

    public override void OnRoomExit()
    {

    }

    public override void OnRoomStay()
    {

    }
}
