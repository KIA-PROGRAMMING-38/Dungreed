using UnityEngine;

public class StartRoom : RoomBase
{
    [field: SerializeField] 
    public InitPosition StartPosition { get; protected set; }

    public override void OnRoomEnter()
    {
    }

    public override void OnRoomExit()
    {
    }

    public override void OnRoomStay()
    {
    }

}
