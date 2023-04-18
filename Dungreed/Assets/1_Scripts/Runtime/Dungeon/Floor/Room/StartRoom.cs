using UnityEngine;

public class StartRoom : RoomBase
{
    [field: SerializeField] 
    public InitPosition StartPosition { get; protected set; }

    public override void OnRoomEnter()
    {
        OnRoomClear?.Invoke();
    }

    public override void OnRoomExit()
    {
    }

    public override void OnRoomStay()
    {
    }

}
