using UnityEngine;

public class StartRoom : RoomBase
{
    [field: SerializeField] 
    public InitPosition StartPosition { get; protected set; }

    public override void RoomEnter()
    {
        Debug.Log("StatRoom");
        OnRoomClear?.Invoke();
    }

    public override void RoomExit()
    {
    }

    public override void RoomUpdate()
    {
    }

}
